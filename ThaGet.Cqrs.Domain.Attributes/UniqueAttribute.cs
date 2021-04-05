using System;

namespace ThaGet.Cqrs.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueAttribute : Attribute
    {
        public UniqueAttribute()
        {
        }

        public UniqueAttribute(string indexGroup)
        {
            IndexGroup = indexGroup;
        }

        public string IndexGroup { get; set; }
    }
}
