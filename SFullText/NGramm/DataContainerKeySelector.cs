using Microsoft.VisualBasic;

namespace SFullText.NGramm
{
    public class DataContainerKeySelector
    {
        public static IEnumerable<string> GetKeysByGroupingQuery(string? groupingQuery)
        {
            if (string.IsNullOrWhiteSpace(groupingQuery))
            {
                yield return "def";
                yield break;
            }

            if (groupingQuery.Contains("<=>"))
            {
                foreach (var key in groupingQuery.Split(new[] { "<=>" }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    yield return key;
                }

                yield break;
            }

            yield return groupingQuery;
        }
    }
}
