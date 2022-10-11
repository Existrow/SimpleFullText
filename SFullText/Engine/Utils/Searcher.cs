using SFullText.Interfaces;
using SFullText.Models;

namespace SFullText.Engine.Utils
{
    public static class Searcher
    {
        public static SearchConfiguration ConfigureSearchQuery() => new();

        public static SearchConfiguration SetSearchQuery(this SearchConfiguration configuration, string query)
        {
            configuration.Query = query;

            return configuration;
        }

        public static SearchConfiguration UseGroupingQuery(this SearchConfiguration configuration, string groupingQuery)
        {
            configuration.GroupingQuery = groupingQuery;

            return configuration;
        }

        public static IEnumerable<T> Search<T>(this SearchConfiguration configuration, IndexStorage<T> storage) where T : ISearchModel
        {
            configuration.ValidateParameters();

            storage = storage ?? throw new ArgumentNullException(nameof(storage));

            if (!storage.DataSourceIsCreated)
                yield break;

            var ids = storage.Index!.Search(configuration);

            foreach (var id in ids)
            {
                if (storage.Storage!.TryGetValue(id, out T? entity))
                    yield return entity;
            }
        }
    }
}
