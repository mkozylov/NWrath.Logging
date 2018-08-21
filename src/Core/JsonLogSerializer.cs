using Newtonsoft.Json;

namespace NWrath.Logging
{
    public class JsonLogSerializer
        : IStringLogSerializer, ILogSerializer
    {
        public JsonSerializerSettings Settings { get; private set; }

        public JsonLogSerializer()
        {
            Settings = new JsonSerializerSettings();
        }

        public JsonLogSerializer(JsonSerializerSettings settings)
        {
            Settings = settings;
        }

        public string Serialize(LogMessage log)
        {
            return JsonConvert.SerializeObject(log, Settings);
        }

        object ILogSerializer.Serialize(LogMessage log)
        {
            return Serialize(log);
        }
    }
}