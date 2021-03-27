using System;
using ThaGet.Cqrs.Sort.Abstractions;
using ThaGet.Cqrs.Sort.Core.Enums;

namespace ThaGet.Cqrs.Sort
{
    public static class SortParser
    {
        public const string ASCENDING_KEY = "asc";
        public const string DESCENDING_KEY = "desc";

        public static ISortInfo Parse(string sortParam)
        {
            var separatorList = new string[] { " " };
            var paramParts = sortParam.Split(separatorList, StringSplitOptions.None);
            
            if (paramParts.Length == 1 || paramParts.Length == 2)
            {
                var descending = paramParts.Length == 2 && paramParts[1] == DESCENDING_KEY;

                return new SortInfo()
                {
                    Property = paramParts[0],
                    Direction = descending 
                        ? SortDirection.Descending 
                        : SortDirection.Ascending
                };
            }

            return null;
        }
    }
}
