using System;
using System.Linq;
using ThaGet.Cqrs.Filter.Abstractions;

namespace ThaGet.Cqrs.Filter
{
    public static class FilterParser
    {
        public static IFilterInfo Parse(string filterParam)
        {
            // = -> like | == -> equal
            var operatorList = new string[] { "==", "!=", "<=", "<", ">=", ">", "=" };
            var paramParts = filterParam.Split(operatorList, StringSplitOptions.None);
            var operatorString = operatorList.FirstOrDefault(o => filterParam.Contains(o));

            if (paramParts.Length == 2)
            {
                return new FilterInfo()
                {
                    Property = paramParts[0],
                    Operator = operatorString,
                    Value = paramParts[1]
                };
            }

            return null;
        }
    }
}