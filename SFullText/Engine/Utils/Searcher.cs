using SFullText.Interfaces;
using SFullText.Models;

namespace SFullText.Engine.Utils
{
    public static class Searcher
    {
        public static IEnumerable<T> SearchByWorld<T>(this IndexStorage<T> indexStorage,
            string world) where T : ISearchModel
        {
            if (!indexStorage.DataSourceIsCreated)
                yield break;

            foreach (var id in indexStorage.Index!.SearchIdsByTerm(world))
            {
                yield return indexStorage.Storage![id];
            }
        }

        public static IEnumerable<T> SearchByTerms<T>(this IndexStorage<T> indexStorage,
            IEnumerable<string> termsСollection, bool containsAllTerms = false) where T : ISearchModel
        {
            if (!indexStorage.DataSourceIsCreated)
                yield break;

            var terms = termsСollection.ToList();
            var serchedIds = new HashSet<int>();

            if (containsAllTerms)
            {
                var result = terms
                    .SelectMany(term => indexStorage.Index!.SearchIdsByTerm(term))
                    .GroupBy(id => id)
                    .Where(group => group.Count() >= terms.Count)
                    .Select(group => group.Key);

                foreach (var id in result)
                    serchedIds.Add(id);
            }
            else
            {
                foreach (var term in terms)
                {
                    foreach (var id in indexStorage.Index!.SearchIdsByTerm(term))
                    {
                        serchedIds.Add(id);
                    }
                }
            }

            foreach (var itemId in serchedIds)
                yield return indexStorage.Storage![itemId];
        }
    }
}
