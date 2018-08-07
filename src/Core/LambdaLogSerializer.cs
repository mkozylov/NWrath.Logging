using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using Newtonsoft.Json;
using System.IO;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using NWrath.Synergy.Common.Structs;

namespace NWrath.Logging
{
    public class LambdaLogSerializer
        : IStringLogSerializer, ILogSerializer
    {
        private Func<LogMessage, string> _stringSerializerFunc;

        private Func<LogMessage, object> _serializerFunc;

        public LambdaLogSerializer(Func<LogMessage, string> stringSerializerFunc)
        {
            _stringSerializerFunc = stringSerializerFunc;

            _serializerFunc = stringSerializerFunc;
        }

        public LambdaLogSerializer(Func<LogMessage, object> serializerFunc)
        {
            _serializerFunc = serializerFunc;

            _stringSerializerFunc = m => serializerFunc(m)?.ToString();
        }

        public virtual string Serialize(LogMessage log)
        {
            return _stringSerializerFunc(log);
        }

        object ILogSerializer.Serialize(LogMessage log)
        {
            return _serializerFunc(log);
        }

        public static implicit operator Func<LogMessage, object>(LambdaLogSerializer serializer)
        {
            return serializer.Serialize;
        }

        public static implicit operator LambdaLogSerializer(string serializerStr)
        {
            return BuildSerializer(serializerStr);
        }

        private static LambdaLogSerializer BuildSerializer(string serializerStr)
        {
            var options = ScriptOptions.Default.AddReferences(
                            typeof(LogMessage).Assembly,
                            typeof(StringSet).Assembly
                            );

            var serializerFunc = CSharpScript.EvaluateAsync<Func<LogMessage, object>>(serializerStr, options).Result;

            return new LambdaLogSerializer(serializerFunc);
        }
    }
}