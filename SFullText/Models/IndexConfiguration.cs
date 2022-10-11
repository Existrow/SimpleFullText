using SFullText.Interfaces;

namespace SFullText.Models;

public class IndexConfiguration
{
    internal List<Func<ISearchModel, string>> LabesPredicates { get; } = new();

    internal bool UseSplitting = true;

    internal void ValidateParameters()
    {
        if (LabesPredicates.Count == 0) LabesPredicates.Add(_ => "def");
    }
}
