using SFullText.Engine;
using SFullText.Interfaces;

namespace SFullText.Models
{
    public class IndexStorage<T> where T : ISearchModel
    {
        internal IReadOnlyDictionary<int, T>? Storage { get; private set; }
        internal IndexBase? Index { get; private set; }

        internal IndexStorage(IReadOnlyDictionary<int, T> storage, IndexBase index)
            => (Storage, Index) = (storage, index);

        public void TrimStorage(bool useGcCollect = true)
        {
            if (DataSourceIsCreated)
            {
                Index!.TrimExcess();
                if (useGcCollect) GC.Collect();
            }
        }

        public bool DataSourceIsCreated
            => Storage != null && Index != null;
    }
}
