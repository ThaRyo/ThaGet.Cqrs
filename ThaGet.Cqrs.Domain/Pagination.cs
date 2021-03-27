using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
using ThaGet.Cqrs.Domain.Abstractions;

namespace ThaGet.Cqrs.Domain
{
    public class Pagination<T> : IPagination<T>
    {
        public IEnumerable<T> Items { get; set; }

        [DefaultValue(-1), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int TotalCount { get; set; } = -1;
    }
}
