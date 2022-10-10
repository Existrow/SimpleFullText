using SFullText.Interfaces;
using SFullText.Models;

namespace SFullText.Engine
{
    internal class NGrammIndex : IndexBase
    {
        internal int NGrammLenght { get; private set; }

        private Dictionary</*ngram*/ string, Dictionary</*groupingKey*/ string, /*ids*/ List<int>>> _indexes = new();

        internal NGrammIndex(int nGrammLenght)
        {
            NGrammLenght = nGrammLenght;
        }

        internal override void Create(IndexConfiguration indexConfiguration, IEnumerable<ISearchModel> searchModels)
        {
            _indexes = new();

            foreach (var model in searchModels)
            {
                string? currentGroupKey = null;
                foreach (var groupingPredicate in indexConfiguration.Labes)
                {
                    currentGroupKey = string.IsNullOrEmpty(currentGroupKey)
                        ? groupingPredicate(model)
                        : $"{currentGroupKey}=>{groupingPredicate(model)}";

                    foreach (var term in model.GetSearchTerms(currentGroupKey))
                    {
                        if (!string.IsNullOrEmpty(term))
                        {
                            AddTermIndex(model.Id, currentGroupKey, term);
                        }
                    }
                }
            }
        }

        internal override IEnumerable<int> SearchIdsByTerm(string term, string groupingKey = "def")
        {
            term = NormailzeTerm(term);
            var scores = new Dictionary<int, int>();

            var trgrmsCount = 0;
            for (int i = 0; i <= term.Length - NGrammLenght; i++)
            {
                trgrmsCount++;
                if (_indexes.TryGetValue(term.Substring(i, NGrammLenght), out var groups))
                {
                    if(groups.TryGetValue(groupingKey, out var ids))
                    {
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
            }

            return scores
                .Where(pair => pair.Value == trgrmsCount)
                .Select(pair => pair.Key);
        }

        internal override void TrimExcess()
        {
            foreach (var groups in _indexes.Values)
            {
                foreach (var model in groups.Values)
                {
                    model.TrimExcess();
                }
                groups.TrimExcess();
            }

            _indexes.TrimExcess();
        }

        private void AddTermIndex(int modelId, string groupKey, string term)
        {
            term = NormailzeTerm(term);

            for (int i = 0; i <= term.Length - NGrammLenght; i++)
            {
                var part = term.Substring(i, NGrammLenght);

                if (_indexes.TryGetValue(part, out var groups))
                {
                    if (groups.TryGetValue(groupKey, out var modelsIds))
                    {
                        if (!modelsIds.Contains(modelId))
                            modelsIds.Add(modelId);
                    }
                    else
                    {
                        groups.Add(groupKey, new() { modelId });
                    }
                }
                else
                {
                    _indexes.Add(part, new() { { groupKey, new() { modelId } } });
                }
            }
        }

        private string NormailzeTerm(string term)
        {
            var capacity = NGrammLenght - term.Length;

            return (capacity > 0
                ? term + string.Join(string.Empty, Enumerable.Repeat(' ', capacity))
                : term).ToLower();
        }
    }
}
