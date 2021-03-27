using ThaGet.Cqrs.Filter.Abstractions;

namespace ThaGet.Cqrs.Filter
{
    public class FilterInfo : IFilterInfo
    {
        public string Property { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }
}