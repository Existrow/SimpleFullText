using SFullText.Interfaces;
using SFullText.Models;

namespace SFullText.Engine
{
    public abstract class IndexBase
    {
        public abstract void Create(IndexConfiguration indexConfiguration, IEnumerable<ISearchModel> searchModels);
        public abstract IEnumerable<uint> Search(SearchConfiguration searchConfiguration);
        public abstract void TrimExcess();
    }
}