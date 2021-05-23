using Healin.Domain.ValueObjects;
using Healin.Shared.Data;
using Healin.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Healin.Domain.Models
{
    public class ExamResult : AuditableEntity
    {
        public string Description { get; private set; }
        public DateTime ExamDate { get; private set; }
        public FilePath FilePath { get; private set; }
        public Guid PatientId { get; private set; }
        public Guid ExamId { get; private set; }

        //EF Relational
        public ICollection<ExamType> ExamTypes { get; private set; }
        public Exam Exam { get; private set; }
        public Patient Patient { get; private set; }

        protected ExamResult() { }

        public ExamResult(string description, DateTime examDate, Guid examId, Guid patientId)
        {
            AssertionConcern.AssertArgumentNotEmpty(description, "A descrição é obrigatória");
            AssertionConcern.AssertArgumentLength(description, 100, "A descrição deve ter no máximo 100 caracteres");

            AssertionConcern.AssertArgumentRange(examDate, DateTime.Now.AddDays(-100), DateTime.Now, "Data do Exame inválida");

            AssertionConcern.AssertArgumentNotEmpty(examId, "O exame é obrigatório");

            AssertionConcern.AssertArgumentNotEmpty(patientId, "O paciente é obrigatório");

            Description = description;
            ExamDate = examDate;
            ExamId = examId;
            PatientId = patientId;
        }

        public void Update(string description, DateTime examDate, Guid examId)
        {
            AssertionConcern.AssertArgumentNotEmpty(description, "A descrição é obrigatória");
            AssertionConcern.AssertArgumentLength(description, 100, "A descrição deve ter no máximo 100 caracteres");

            AssertionConcern.AssertArgumentRange(examDate, DateTime.Now.AddDays(-100), DateTime.Now, "Data do Exame inválida");

            AssertionConcern.AssertArgumentNotEmpty(examId, "O exame é obrigatório");

            Description = description;
            ExamDate = examDate;
            ExamId = examId;
        }

        public void UpdateExamTypes(ICollection<ExamType> examTypes)
        {
            if (ExamTypes is null)
            {
                ExamTypes = new List<ExamType>();
            }

            var toAddExamTypes = examTypes.Where(e => ExamTypes.All(et => et.Id != e.Id)).ToList();
            var toRemoveExamTypes = ExamTypes.Where(e => examTypes.All(et => et.Id != e.Id)).ToList();

            toAddExamTypes.ForEach(add => ExamTypes.Add(add));
            toRemoveExamTypes.ForEach(remove => ExamTypes.Remove(remove));
        }

        public void AddFilePath(string filePath) => FilePath = FilePath.CreatePDF(filePath);
    }
}
