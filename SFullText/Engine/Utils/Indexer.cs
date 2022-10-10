using SFullText.Interfaces;
using SFullText.Models;

namespace SFullText.Engine.Utils
{
    public static class Indexer
    {
        public static IndexConfiguration ConfigureIndex() => new();

        public static IndexConfiguration GroupByLabels(this IndexConfiguration indexConfiguration, Func<ISearchModel, string> labelPredicate)
        {
            indexConfiguration.Labes.Add(labelPredicate);

            return indexConfiguration;
        }

        public static IndexStorage<T> CreateNgrammIndex<T>(this IndexConfiguration indexConfiguration, IReadOnlyDictionary<int, T> storgae, int ngrammLenght = 3) where T : ISearchModel
        {
            var index = new NGrammIndex(ngrammLenght);
            index.Create(indexConfiguration, storgae!.Values.Select(sp => (ISearchModel)sp));

            return new(storgae, index);
        }

        public static IndexStorage<T> CreateNgrammIndex<T>(this IndexConfiguration indexConfiguration, IEnumerable<T> searchModels, int ngrammLenght = 3) where T : ISearchModel
        {
            var storgae = new Dictionary<int, T>();
            foreach (var searchModel in searchModels)
            {
                storgae.TryAdd(searchModel.Id, searchModel);
            }

            var index = new NGrammIndex(ngrammLenght);
            index.Create(indexConfiguration, storgae!.Values.Select(sp => (ISearchModel)sp));

            return new(storgae, index);
        }
    }
}
