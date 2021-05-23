using System;
using System.Collections.Generic;

namespace Healin.Application.Requests
{
    public class ExamResultRequest : RequestBase
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime ExamDate { get; set; }
        public Guid ExamId { get; set; }
        public IEnumerable<Guid> ExamTypeIds { get; set; }
    }
}
