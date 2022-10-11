namespace SFullText.Models
{
    public class SearchConfiguration
    {
        internal string? Query { get; set; }

        internal bool UseSplitting { get; set; } = true;

        internal string? GroupingQuery { get; set; }

        internal void ValidateParameters()
        {
            if (string.IsNullOrWhiteSpace(GroupingQuery)) GroupingQuery = "def";
        }
    }
}
