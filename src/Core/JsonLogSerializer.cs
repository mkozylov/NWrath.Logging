using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using Newtonsoft.Json;
using System.IO;

namespace NWrath.Logging
{
    public class JsonLogSerializer
        : IStringLogSerializer, ILogSerializer
    {
        public virtual JsonSerializerSettings Settings { get; private set; }

        public JsonLogSerializer()
        {
            Settings = new JsonSerializerSettings();
        }

        public JsonLogSerializer(JsonSerializerSettings settings)
        {
            Settings = settings;
        }

        public virtual string Serialize(LogMessage log)
        {
            return JsonConvert.SerializeObject(log, Settings);
        }

        object ILogSerializer.Serialize(LogMessage log)
        {
            return Serialize(log);
        }
    }
}