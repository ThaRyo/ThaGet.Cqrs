using ThaGet.Cqrs.Sort.Abstractions;
using ThaGet.Cqrs.Sort.Core.Enums;

namespace ThaGet.Cqrs.Sort
{
    public class SortInfo : ISortInfo
    {
        public string Property { get; set; }
        public SortDirection Direction { get; set; }
    }
}
