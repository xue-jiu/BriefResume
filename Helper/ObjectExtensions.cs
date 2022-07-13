using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BriefResume.Helper
{
    public static class ObjectExtensions
    {
        public static ExpandoObject ShapeData<Tsource>(this Tsource source,string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            var dataShapedObject = new ExpandoObject();
            //反射消耗比较大,处理反射机制避免遍历列表
            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(Tsource)
                    .GetProperties(BindingFlags.IgnoreCase |
                    BindingFlags.Public | BindingFlags.Instance);

                foreach (var propertyInfo in propertyInfos)
                {
                    // get the value of the property on the source object
                    var propertyValue = propertyInfo.GetValue(source);

                    // add the field to the ExpandoObject
                    ((IDictionary<string, object>)dataShapedObject)
                        .Add(propertyInfo.Name, propertyValue);
                }
                return dataShapedObject;
            }
            else
            {
                var fieldsAfterSplit = fields.Split(",");
                foreach (var field in fieldsAfterSplit)
                {
                    var PropertyName = field.Trim();
                    var PropertyInfo = typeof(Tsource).GetProperty(PropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (PropertyInfo == null) 
                    {
                        throw new Exception($"找不到属性{PropertyInfo}");
                    }
                    var propertyValue = PropertyInfo.GetValue(source);
                    ((IDictionary<string, object>)dataShapedObject).Add(PropertyInfo.Name, propertyValue);
                }
            }
            return dataShapedObject;
        }
    }
}
