using SFullText.Interfaces;

namespace SFullText.Engine
{
    internal class NGrammIndex : IndexBase
    {
        internal int NGrammLenght { get; private set; }

        private Dictionary<string, /*ids*/ List<int>> _indexes = new();

        internal NGrammIndex(int nGrammLenght)
        {
            NGrammLenght = nGrammLenght;
        }

        internal override void Create(IEnumerable<ISearchModel> searchModels)
        {
            _indexes = new();

            foreach (var model in searchModels)
            {
                foreach (var term in model.SearchTerms)
                {
                    AddTermIndex(term, model.Id);
                }
            }
        }

        internal override IEnumerable<int> SearchIdsByTerm(string term)
        {
            var scores = new Dictionary<int, int>();

            var trgrmsCount = 0;
            for (int i = 0; i <= term.Length - NGrammLenght; i++)
            {
                trgrmsCount++;
                if (_indexes.TryGetValue(term.Substring(i, NGrammLenght), out var ids))
                {
                    foreach(var id in ids)
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
                .Where(pair => pair.Value == trgrmsCount)
                .Select(pair => pair.Key);
        }

        internal override void TrimExcess()
        {
            foreach (var models in _indexes.Values)
            {
                models.TrimExcess();
            }

            _indexes.TrimExcess();
        }

        private void AddTermIndex(string term, int modelId)
        {
            for (int i = 0; i <= term.Length - NGrammLenght; i++)
            {
                var part = term.Substring(i, NGrammLenght);

                if (_indexes.TryGetValue(part, out var modelsIds))
                {
                    if (!modelsIds.Contains(modelId))
                        modelsIds.Add(modelId);
                }
                else
                {
                    _indexes.Add(part, new() { modelId });
                }
            }
        }
    }
}
