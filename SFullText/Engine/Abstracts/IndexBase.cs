using SFullText.Interfaces;

namespace SFullText.Engine
{
    internal abstract class IndexBase
    {
        internal abstract void Create(IEnumerable<ISearchModel> searchModels);
        internal abstract IEnumerable<int> SearchIdsByTerm(string term);
        internal abstract void TrimExcess();
    }
}