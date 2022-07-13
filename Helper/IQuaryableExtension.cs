using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace BriefResume.Helper
{
    public static class IQuaryableExtension
    {
        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T>source,
            string orderBy,
            Dictionary<string,  PropertyMappingValue> mappingDictionary)
        {
            if (source==null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (mappingDictionary == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByAfterSplite = orderBy.Split(",");//先区分一下是否需要 逆向排序
            foreach (var orderByClause in orderByAfterSplite.Reverse())
            {
                var TrimedOrderBy=orderByClause.Trim();
                var OrderDescending = TrimedOrderBy.EndsWith(" desc");//感觉也可以用正则表达式
                var IndexOfFirstSpace = TrimedOrderBy.IndexOf(" ", StringComparison.Ordinal);//找到第一个匹配" "的索引,若没有则为-1
                var propertyName = IndexOfFirstSpace ==-1 ? TrimedOrderBy : TrimedOrderBy.Remove(IndexOfFirstSpace);
                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"没有找到key为{propertyName}的映射");
                }
                var propertyMappingValue = mappingDictionary[propertyName];
                if (propertyMappingValue==null)
                {
                    throw new ArgumentException($"{nameof(propertyMappingValue)}不存在");
                }
                foreach (var DestinationPropertie in propertyMappingValue.DestinationProperties.Reverse())//为啥要反转
                {
                    if (propertyMappingValue.Revert)
                    {
                        OrderDescending = !OrderDescending;
                    }
                    source = source.OrderBy(DestinationPropertie + (OrderDescending ? " descending" : " ascending"));
                }
            }
            return source;
        }
    }
}
