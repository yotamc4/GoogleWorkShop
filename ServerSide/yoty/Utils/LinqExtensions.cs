using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Utils
{
    public static class LinqExtensions
    {
        public static IEnumerable<TSource> Sort<TSource,TKey>(this IEnumerable<TSource> collection,Func<TSource,TKey> sortingKey, BidsSortByOrder order)
        {
            switch (order)
            {
                case BidsSortByOrder.ACS:
                    return collection.OrderBy(sortingKey);
                default:
                    return collection.OrderByDescending(sortingKey);
            }
        }
    }
}
