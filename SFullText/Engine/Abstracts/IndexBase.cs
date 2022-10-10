using SFullText.Interfaces;
using SFullText.Models;

namespace SFullText.Engine
{
    internal abstract class IndexBase
    {
        internal abstract void Create(IndexConfiguration indexConfiguration, IEnumerable<ISearchModel> searchModels);
        internal abstract IEnumerable<int> SearchIdsByTerm(string term, string groupingKey = "def");
        internal abstract void TrimExcess();
    }
}