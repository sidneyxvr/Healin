using System;
using System.Collections.Generic;

namespace Healin.Application.Responses
{
    public class ExamResultResponse
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime ExamDate { get; set; }
        public Guid ExamId { get; set; }
        public string Exam { get; set; }
        public string FilePath { get; set; }
        public IEnumerable<SelectItem<Guid>> ExamTypes { get; set; }
    }
}
