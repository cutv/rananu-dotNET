using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rananu.Shared
{
    public class NewtonsoftConvert
    {
        public JsonSerializerSettings _settings;
        public NewtonsoftConvert()
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

        private static readonly Lazy<NewtonsoftConvert> _lazy = new Lazy<NewtonsoftConvert>(() => new NewtonsoftConvert());
        public static NewtonsoftConvert Instance => _lazy.Value;

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
