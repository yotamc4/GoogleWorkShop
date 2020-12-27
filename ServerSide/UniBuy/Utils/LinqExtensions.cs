// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using UniBuy.WebApi.PublicDataSchemas;
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
