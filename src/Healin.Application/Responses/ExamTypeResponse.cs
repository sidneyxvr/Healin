using System;

namespace Healin.Application.Responses
{
    public class ExamTypeResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Guid ExamId { get; set; }
        public ExamResponse Exam { get; set; }
    }
}
