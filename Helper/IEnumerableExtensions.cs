using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BriefResume.Helper
{
    public static class IEnumerableExtensions//静态类
    {
        public static IEnumerable<ExpandoObject> ShapeData<Tsource>(//使用expando也是为了解耦,避免了显式类型转换
            this IEnumerable<Tsource> source,
            string fields
            )
        {
            if (source==null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            var ExpandoObjectList = new List<ExpandoObject>();
            //反射消耗比较大,处理反射机制避免遍历列表
            var propertyInfoList = new List<PropertyInfo>();//PropertyInfo包含对象属性所有的信息
            if (string.IsNullOrWhiteSpace(fields))
            {
                //保存所有的属性
                var propertyInfos = typeof(Tsource).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);//默认只能查找到Public方法
                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var fieldsAfterSplit = fields.Split(",");
                foreach (var field in fieldsAfterSplit)
                {
                    var PropertyName = field.Trim();
                    var PropertyInfo = typeof(Tsource).GetProperty(PropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (PropertyInfo==null)
                    {
                        throw new Exception($"找不到属性{PropertyInfo}");
                    }
                    propertyInfoList.Add(PropertyInfo);
                }
            }
            foreach (var sourceObject in source)
            {
                var dataShapedObject = new ExpandoObject();
                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);//直接用IDictionary如何?
                }
                ExpandoObjectList.Add(dataShapedObject);
            }
            return ExpandoObjectList;

        }
    }
}
