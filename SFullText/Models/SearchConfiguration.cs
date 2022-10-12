namespace SFullText.Models
{
    public class SearchConfiguration
    {
        internal string? Query { get; set; }

        internal bool UseSplitting { get; set; } = true;

        internal List<string> Groups { get; set; } = new();

        internal void ValidateParameters()
        {
            if (Groups.Count == 0) Groups.Add("def");
        }
    }
}
