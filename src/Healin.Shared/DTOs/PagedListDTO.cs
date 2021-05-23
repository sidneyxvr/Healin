using System.Collections.Generic;

namespace Healin.Shared.DTOs
{
    public class PagedListDTO<T>
    {
        public ICollection<T> Collection { get; }
        public int Amount { get; }

        public PagedListDTO(ICollection<T> collection, int amount)
        {
            Collection = collection;
            Amount = amount;
        }
    }
}
