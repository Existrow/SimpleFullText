namespace SFullText.Interfaces
{
    public interface ISearchModel
    {
        public int Id { get; }

        public IEnumerable<string> GetSearchTerms(string groupKey);
    }
}
