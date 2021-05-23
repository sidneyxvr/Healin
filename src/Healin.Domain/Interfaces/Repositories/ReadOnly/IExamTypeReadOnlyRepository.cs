﻿using Healin.Domain.Models;
using Healin.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Domain.Interfaces.Repositories.ReadOnly
{
    public interface IExamTypeReadOnlyRepository
    {
        Task<IList<ExamType>> GetByExamIdAsync(Guid examId);
        Task<PagedListDTO<ExamType>> GetPagedAsync(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "");
    }
}
