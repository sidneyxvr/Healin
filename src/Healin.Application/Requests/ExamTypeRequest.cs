using System;

namespace Healin.Application.Requests
{
    public class ExamTypeRequest : RequestBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Guid ExamId { get; set; }
    }
}
