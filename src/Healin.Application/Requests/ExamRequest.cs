using System;

namespace Healin.Application.Requests
{
    public class ExamRequest : RequestBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
