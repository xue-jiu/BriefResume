using BriefResume.Dtos;
using BriefResume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BriefResume.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _SeekerAttributePropertyMapping =
           new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
           {
               { "ExpadSalary", new PropertyMappingValue(new List<string>(){ "ExpadSalary" }) },
               { "Degree", new PropertyMappingValue(new List<string>(){ "Degree" })},
           };

        private Dictionary<string, PropertyMappingValue> _SeekerPropertyMapping =
           new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
           {
                { "Email", new PropertyMappingValue(new List<string>(){ "Email" }) },
                { "PhoneNumber", new PropertyMappingValue(new List<string>(){ "PhoneNumber" })},
           };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(
                new PropertyMapping<SeekerAttributeDto, SeekerAttribute>(_SeekerAttributePropertyMapping));
            _propertyMappings.Add(
                new PropertyMapping<SeekerDto, Seeker>(_SeekerPropertyMapping));
        }


        public Dictionary<string, PropertyMappingValue>
            GetPropertyMapping<TSource, TDestination>()
        {
            // 获得匹配的映射对象
            var matchingMapping =
                _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception(
                $"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool IsMappingExists<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }
            //逗号来分隔字段字符串
            var fieldsAfterSplit = fields.Split(",");
            foreach (var field in fieldsAfterSplit)
            {
                // 去掉空格
                var trimmedField = field.Trim();
                // 获得属性名称字符串
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsPropertiesExists<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }
            //逗号来分隔字段字符串
            var fieldsAfterSplit = fields.Split(',');
            foreach(var field in fieldsAfterSplit)
            {
                // 获得属性名称字符串
                var propertyName = field.Trim();
                var propertyInfo = typeof(T)
                    .GetProperty(
                        propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public
                        | BindingFlags.Instance
                    );
                // 如果T中没找到对应的属性
                if(propertyInfo == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
