namespace SFullText.NGramm
{
    public class DataContainer
    {
        private readonly Dictionary</*NGrammId*/ uint, /*EntitiesIds*/ List<uint>> _data = new();

        internal void AddNgrammAssociation(uint nGrammId, uint entityId)
        {
            if (_data.TryGetValue(nGrammId, out var idsCollection))
            {
                if (!idsCollection.Contains(entityId))
                {
                    idsCollection.Add(entityId);
                }
            }
            else
            {
                _data.Add(nGrammId, new() { entityId });
            }
        }

        public IEnumerable<uint> GetEntitiesIdsByNGramm(uint nGrammId)
        {
            if (_data.TryGetValue(nGrammId, out var idsCollection))
            {
                return idsCollection;
            }

            return Enumerable.Empty<uint>();
        }

        public void TrimExcess()
        {
            foreach (var collection in _data.Values)
            {
                collection.TrimExcess();
            }

            _data.TrimExcess();
        }
    }
}
