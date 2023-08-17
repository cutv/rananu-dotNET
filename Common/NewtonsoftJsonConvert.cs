using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class NewtonsoftJsonConvert
    {
        public JsonSerializerSettings _settings;
        public NewtonsoftJsonConvert()
        {
            _settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };
        }

        private static readonly Lazy<NewtonsoftJsonConvert> _lazy = new Lazy<NewtonsoftJsonConvert>(() => new NewtonsoftJsonConvert());
        public static NewtonsoftJsonConvert Instance => _lazy.Value;

        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }

        public object? DeserializeObject(string json)
        {
            return JsonConvert.DeserializeObject(json, _settings);
        }

        public JsonSerializerSettings Settings => _settings;
    }
}
