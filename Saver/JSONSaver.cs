using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Trace.Saver
{
    public class JSONSaver:Saver
    {
        private JToken Save(MethodTracerResult methodTracerResult)
        {
            var savedTracedMetod = new JObject
            {
                { "name", methodTracerResult.MethodName },
                { "class", methodTracerResult.ClassName },
                { "time", methodTracerResult.ElapsedTime.Milliseconds + "ms" }
            };

            if (methodTracerResult.Inner.Any())
                savedTracedMetod.Add("methods", new JArray(from mt in methodTracerResult.Inner
                                                           select Save(mt)));
            return savedTracedMetod;
        }

        private JToken Save(ThreadTracerResult threadTracerResult)
        {
            var extremeMethods = from method in threadTracerResult.ExtremeMethodResults
                                 select Save(method);
            return new JObject
            {
                { "id", threadTracerResult.ThreadId },
                { "time", threadTracerResult.TimeElapsed.Milliseconds + "ms"},
                { "methods", new JArray(extremeMethods) }
            };
        }

        override public void SaveTraceResult(TextWriter textWriter, TraceResult traceResult)
        {
            var sortedThreadTracerResults = from item in traceResult.ThreadTracerResults
                                      orderby item.Key
                                      select item.Value;
            var jtokens = from threadTracerResult in sortedThreadTracerResults
                     select Save(threadTracerResult);
            JObject traceResultJSON = new JObject
            {
                { "threads", new JArray(jtokens) }
            };
            using (var jsonWriter = new JsonTextWriter(textWriter))
            {
                jsonWriter.Formatting = Formatting.Indented;
                traceResultJSON.WriteTo(jsonWriter);
            }
        }
    }
}
