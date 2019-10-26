using Newtonsoft.Json;

namespace NWrath.Logging
{
    public class JsonLogSerializer
        : IStringLogSerializer
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

        public string Serialize(LogRecord record)
        {
            return JsonConvert.SerializeObject(record, Settings);
        }
    }
}