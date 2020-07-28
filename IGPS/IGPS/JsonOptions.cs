using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IGPS
{
    public static class JsonOptions
    {
        public static JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
    }
}
