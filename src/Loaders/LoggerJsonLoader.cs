using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NWrath.Synergy.Common.Extensions;
using System;
using System.IO;
using System.Linq;
using NWrath.Synergy.Common.Structs;
using NWrath.Synergy.Reflection.Extensions;

namespace NWrath.Logging
{
    public class LoggerJsonLoader
    {
        public IServiceProvider Injector { get; set; }

        private FactoryUnitList<JToken, Type, object> _factories = new FactoryUnitList<JToken, Type, object>();
        private bool _checkMemberExistence;

        public LoggerJsonLoader(bool checkMemberExistence = true)
        {
            _checkMemberExistence = checkMemberExistence;

            _factories.Add(x => x.Type == JTokenType.Object, (x, t) => GetJObjectValue(x, t));
            _factories.Add(x => x.Type == JTokenType.Array, (x, t) => GetJArrayValue(x, t));
            _factories.Add(x => true, (x, t) => GetJValueValue(x, t));
        }

        public ILogger Load(JToken loggingSection)
        {
            var loggerCollectionSection = loggingSection.Children()
                                                        .ToList();

            if (loggerCollectionSection.Count == 1)
            {
                var info = loggerCollectionSection.First();

                var logger = CreateLoggerFromJson(info);

                return logger;
            }
            else
            {
                var loggers = loggerCollectionSection.Select(x => CreateLoggerFromJson(x))
                                                     .ToArray();

                var compositeLogger = new CompositeLogger(loggers);

                return compositeLogger;
            }
        }

        public ILogger Load(string filePath, string sectionPath)
        {
            using (var sr = new StreamReader(filePath))
            using (var jr = new JsonTextReader(sr))
            {
                var json = JObject.Load(jr);

                var section = json.SelectToken(sectionPath, true);

                return Load(section);
            }
        }

        private ILogger CreateLoggerFromJson(JToken info)
        {
            var loggerProp = (JProperty)info;

            var loggerSection = new JObject(loggerProp);

            var loggerType = Type.GetType(loggerProp.Name);

            var loggerInstance = (ILogger)_factories.First(f => f.Predicate(loggerSection))
                                                    .Produce(loggerSection, loggerType);

            return loggerInstance;
        }

        private object GetJObjectValue(JToken token, Type expectedType = null)
        {
            var obj = token.CastTo<JObject>();
            var prop = obj.First.CastAs<JProperty>();

            if (obj.Count == 1 && prop != null)
            {
                if (prop.Name == "$")
                {
                    return GetInjectorService(prop);
                }
                else if (prop.Name.Contains("."))
                {
                    return CreateInstanceByClassInfo(prop);
                }
            }

            return expectedType != null
                   ? obj.ToObject(expectedType)
                   : obj;
        }

        private object GetJValueValue(JToken token, Type expectedType = null)
        {
            return token.CastTo<JValue>().Value;
        }

        private object GetJArrayValue(JToken token, Type expectedType = null)
        {
            return token.ToObject(expectedType);
        }

        private object CreateInstanceByClassInfo(JProperty prop)
        {
            var type = Type.GetType(prop.Name, true);

            var ctorProp = prop.Value.SelectToken("$ctor");

            var instance = default(object);

            instance = ctorProp == null
                         ? prop.Value.ToObject(type)
                         : CreateInstanceByCtorInfo(ctorProp, type);

            AssignInstanceMembers(prop, type, instance);

            return instance;
        }

        private object GetInjectorService(JProperty prop)
        {
            Injector.Required(() => throw new ArgumentNullException(nameof(Injector)));

            var val = prop.Value.CastTo<JValue>()
                                     .Value<string>();

            var type = Type.GetType(val);

            return Injector.GetService(type);
        }

        private object CreateInstanceByCtorInfo(JToken ctorProp, Type type)
        {
            var expectedCtorArgsCount = ctorProp.Count();

            var ctorArgTypes = ctorProp.Select(t => t.CastAs<JValue>()?.Value?.GetType()
                                                     ?? Type.GetType(t.FirstOrDefault().CastAs<JProperty>()?.Name ?? "-")
                                                     ?? Type.GetType(t.FirstOrDefault().CastAs<JProperty>()?.Value.CastAs<JValue>()?.Value?.ToString() ?? "-"))
                                           .ToList();

            var ctorParams = type.GetConstructors()
                                 .FirstOrDefault(c =>
                                 {
                                     var parameters = c.GetParameters();

                                     return expectedCtorArgsCount == parameters.Length
                                            && (parameters.Length == 0 || ctorArgTypes.Select((x, i) => x == null || (parameters.ElementAtOrDefault(i)?.ParameterType?.IsAssignableFrom(x) ?? false))
                                                                                      .All(x => x));
                                 })
                                 ?.GetParameters();

            var args = ctorParams == null
                        ? new object[] { }
                        : ctorProp.Select((t, i) => _factories.First(f => f.Predicate(t)).Produce(t, ctorParams.ElementAtOrDefault(i)?.ParameterType))
                                  .ToArray();

            return Activator.CreateInstance(type, args);
        }

        private void AssignInstanceMembers(JProperty prop, Type type, object instance)
        {
            var members = type.GetPublicMembers();

            var jsonProps = prop.Value.Cast<JProperty>().Where(x => x.Name[0] != '$').ToArray();

            foreach (JProperty propInfo in jsonProps)
            {
                if (propInfo.Name.Equals("level", StringComparison.OrdinalIgnoreCase)
                    && typeof(ILogger).IsAssignableFrom(type))
                {
                    ConfigureLevelVerifier(propInfo, (ILogger)instance);
                }
                else
                {
                    var member = members.FirstOrDefault(x => x.Name.Equals(propInfo.Name, StringComparison.OrdinalIgnoreCase));

                    if (member == null)
                    {
                        if (_checkMemberExistence)
                        {
                            throw new ArgumentNullException(nameof(member), $"'{type}' does not contain member '{propInfo.Name}'");
                        }
                        else
                        {
                            continue;
                        }
                    }

                    var memberType = member.GetMemberType();

                    memberType = (memberType.IsInterface || memberType.IsAbstract)
                                    ? (member.GetMemberValue(instance)?.GetType() ?? memberType)
                                    : memberType;

                    var val = _factories.First(f => f.Predicate(propInfo.Value)).Produce(propInfo.Value, memberType);

                    member.SetMemberValue(instance, val);
                }
            }
        }

        private void ConfigureLevelVerifier(JProperty levelJProp, ILogger loggerInstance)
        {
            var jvalue = levelJProp.Value;

            switch (jvalue.Type)
            {
                case JTokenType.String:
                    loggerInstance.RecordVerifier = new MinimumLogLevelVerifier(
                        ParseLogLevel(jvalue.Value<string>())
                        );
                    break;

                case JTokenType.Object:
                    var rangeLevel = (JProperty)jvalue.First;

                    loggerInstance.RecordVerifier = new RangeLogLevelVerifier(
                        ParseLogLevel(rangeLevel.Name),
                        ParseLogLevel(rangeLevel.Value.ToString())
                        );
                    break;

                case JTokenType.Array:
                    loggerInstance.RecordVerifier = new MultipleLogLevelVerifier(
                        jvalue.Values<string>().Select(x => ParseLogLevel(x)).ToArray()
                        );
                    break;

                default:
                    throw new Exception($"Can`t perform '{levelJProp.Parent.Path}' 'Level' section");
            }
        }

        private static LogLevel ParseLogLevel(string val)
        {
            return (LogLevel)Enum.Parse(typeof(LogLevel), val);
        }
    }
}