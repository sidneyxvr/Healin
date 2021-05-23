using System.Collections.Generic;

namespace Healin.Infrastructure.Storage
{
    public class StorageResult<T>
    {
        public bool Succeeded { get; set; }
        public T Result { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
