using SFullText.Engine;
using SFullText.Interfaces;
using SFullText.Models;

namespace SFullText.NGramm
{
    public class NGrammIndex : IndexBase
    {
        public int NGrammLength { get; private set; }

        private readonly Dictionary</*NGramm*/ string, /*NGrammId*/ uint> _nGramms = new();

        private readonly Dictionary</*Key*/ string, /*Data*/ DataContainer> _dataContainers = new();

        private uint _currentNgrammId;

        public NGrammIndex(int nGrammLength)
        {
            NGrammLength = nGrammLength;
        }

        public override void Create(IndexConfiguration indexConfiguration, IEnumerable<ISearchModel> searchModels)
        {
            foreach (var model in searchModels)
            {
                foreach (var containerKeyPredicate in indexConfiguration.LabesPredicates)
                {
                    var containerKey = containerKeyPredicate(model);
                    var hasContainer = _dataContainers.TryGetValue(containerKey, out var container);
                    var dataContainer = hasContainer ? container : new();

                    var searchTerms = indexConfiguration.UseSplitting
                        ? model.GetSearchTerms(containerKey).SelectMany(term => term?.Split() ?? Enumerable.Empty<string>())
                        : model.GetSearchTerms(containerKey);

                    FillDataForModel(ref dataContainer!, searchTerms, model.Id);

                    if (!hasContainer) _dataContainers.Add(containerKey, dataContainer!);
                }
            }
        }

        public override IEnumerable<uint> Search(SearchConfiguration searchConfiguration)
        {
            var searchedNGramms = searchConfiguration.UseSplitting
                ? searchConfiguration.Query?.Split().SelectMany(term => NGrammStringSplitter.SplitString(term, NGrammLength))
                : NGrammStringSplitter.SplitString(searchConfiguration.Query, NGrammLength);
            searchedNGramms ??= Enumerable.Empty<string>();

            var usedContainers = GetContainers(searchConfiguration.Groups);

            var scores = new Dictionary<uint, int>();

            var ngrammsCount = 0;
            foreach (var ngramm in searchedNGramms)
            {
                ngrammsCount++;

                if (_nGramms.TryGetValue(ngramm, out var nGrammId))
                {
                    var ids = usedContainers.SelectMany(container => container.GetEntitiesIdsByNGramm(nGrammId)).Distinct();

                    foreach (var id in ids)
                    {
                        if (scores.TryGetValue(id, out var score))
                        {
                            scores[id] = ++score;
                        }
                        else
                        {
                            scores.Add(id, 1);
                        };
                    }
                }
            }

            return scores
                .Where(pair => pair.Value >= ngrammsCount)
                .Select(pair => pair.Key);
        }

        private IEnumerable<DataContainer> GetContainers(IEnumerable<string> groups)
        {
            foreach (var containerKey in groups)
            {
                if (_dataContainers.TryGetValue(containerKey, out var container)) yield return container;
            }
        }

        private void FillDataForModel(ref DataContainer dataContainer, IEnumerable<string> terms, uint entityId)
        {
            foreach(var term in terms)
            {
                foreach (var ngramm in NGrammStringSplitter.SplitString(term, NGrammLength))
                {
                    var nGrammId = AddNgramm(ngramm);
                    dataContainer.AddNgrammAssociation(nGrammId, entityId);
                }
            }
        }

        private uint AddNgramm(string nGramm)
        {
            if (_nGramms.TryGetValue(nGramm, out var id))
            {
                return id;
            }
            else
            {
                _nGramms.Add(nGramm, _currentNgrammId);
                return _currentNgrammId++;
            }
        }

        public override void TrimExcess()
        {
            foreach (var dataContainer in _dataContainers.Values)
            {
                dataContainer.TrimExcess();
            }
            _nGramms.TrimExcess();
            _dataContainers.TrimExcess();
        }
    }
}
