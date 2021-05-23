﻿using Healin.Domain.Models;
using Healin.Shared.Intefaces;
using System.Threading.Tasks;

namespace Healin.Domain.Interfaces.Repositories
{
    public interface IPrescriptionRepository : IRepository<Prescription>
    {
        Task RemoveAsync(Prescription prescription);
    }
}
