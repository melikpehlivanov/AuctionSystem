namespace Domain.Common
{
    using System;

    public class AuditableEntity
    {
        public string CreatedBy { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public string LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}