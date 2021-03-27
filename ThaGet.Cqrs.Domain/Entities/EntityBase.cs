using System;
using System.ComponentModel.DataAnnotations;
using ThaGet.Cqrs.Domain.Entities.Abstractions;

namespace ThaGet.Cqrs.Domain.Entities
{
    public abstract class EntityBase<TId> : IEntity<TId>
        where TId : struct
    {
        protected EntityBase()
        {
            CreatedAtUtc = DateTime.UtcNow;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public TId Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
