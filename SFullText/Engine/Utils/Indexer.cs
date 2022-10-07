using SFullText.Interfaces;
using SFullText.Models;

namespace SFullText.Engine.Utils
{
    public static class Indexer
    {
        public static IndexStorage<T> CreateNgrammIndex<T>(IReadOnlyDictionary<int, T> storgae, int ngrammLenght = 3) where T : ISearchModel
        {
            var index = new NGrammIndex(ngrammLenght);
            index.Create(storgae!.Values.Select(sp => (ISearchModel)sp));

            return new(storgae, index);
        }

        public static IndexStorage<T> CreateNgrammIndex<T>(IEnumerable<T> searchModels, int ngrammLenght = 3) where T : ISearchModel
        {
            var storgae = new Dictionary<int, T>();
            foreach (var searchModel in searchModels)
            {
                storgae.TryAdd(searchModel.Id, searchModel);
            }

            var index = new NGrammIndex(ngrammLenght);
            index.Create(storgae!.Values.Select(sp => (ISearchModel)sp));

            return new(storgae, index);
        }
    }
}
