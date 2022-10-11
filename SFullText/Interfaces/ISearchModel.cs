namespace SFullText.Interfaces
{
    public interface ISearchModel
    {
        public uint Id { get; }

        public IEnumerable<string> GetSearchTerms(string groupKey);
    }
}
