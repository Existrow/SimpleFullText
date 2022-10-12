using SFullText.Interfaces;
using SFullText.Models;
using SFullText.NGramm;

namespace SFullText.Engine.Utils
{
    public static class Indexer
    {
        public static IndexConfiguration ConfigureIndex() => new();

        public static IndexConfiguration GroupByLabels(this IndexConfiguration indexConfiguration, Func<ISearchModel, string> labelPredicate)
        {
            indexConfiguration.LabesPredicates.Add(labelPredicate);

            return indexConfiguration;
        }

        public static IndexStorage<T> CreateNgrammIndex<T>(this IndexConfiguration indexConfiguration, IReadOnlyDictionary<uint, T> storgae, int ngrammLenght = 3) where T : ISearchModel
        {
            indexConfiguration.ValidateParameters();

            var index = new NGrammIndex(ngrammLenght);
            index.Create(indexConfiguration, storgae!.Values.Select(sp => (ISearchModel)sp));

            return new(storgae, index);
        }

        public static IndexStorage<T> CreateNgrammIndex<T>(this IndexConfiguration indexConfiguration, IEnumerable<KeyValuePair<uint, T>> searchModels, int ngrammLenght = 3) where T : ISearchModel
        {
            indexConfiguration.ValidateParameters();

            var storgae = new Dictionary<uint, T>();
            foreach (var searchModel in searchModels)
            {
                storgae.TryAdd(searchModel.Key, searchModel.Value);
            }

            var index = new NGrammIndex(ngrammLenght);
            index.Create(indexConfiguration, storgae!.Values.Select(sp => (ISearchModel)sp));

            return new(storgae, index);
        }
    }
}
