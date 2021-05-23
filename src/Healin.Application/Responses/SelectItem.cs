using System;

namespace Healin.Application.Responses
{
    public class SelectItem<T>
    {
        public T Value { get; set; }
        public string Text { get; set; }
    }
}
