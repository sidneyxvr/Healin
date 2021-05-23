using System;

namespace Healin.Shared.Data
{
    public abstract class AuditableEntity : Entity
    {
        public Guid? CreatedById { get; private set; }
        public DateTime Created { get; private set; }
        public Guid? LastModifiedById { get; private set; }
        public DateTime? LastModified { get; private set; }
    }
}
