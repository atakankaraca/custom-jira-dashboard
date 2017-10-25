using HürriyetJiraDashboard.Models.JsonClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;

namespace JiraDashboard.Models.Helpers
{
    public class JsonSettings
    {
        public class CustomDataContractResolver : DefaultContractResolver
        {
            public static readonly CustomDataContractResolver Instance = new CustomDataContractResolver();            
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);
                if (property.DeclaringType == typeof(Fields))
                {
                    if (property.PropertyName.Equals("StoryPoint", StringComparison.OrdinalIgnoreCase))
                    {
                        property.PropertyName = GenericHelper.storypointfield;
                    }
                }
                return property;
            }
        }
    }
}