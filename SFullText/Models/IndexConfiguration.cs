using SFullText.Interfaces;

namespace SFullText.Models;

public class IndexConfiguration
{
    internal List<Func<ISearchModel, string>> Labes { get; } = new() { _ => "def" };
}
