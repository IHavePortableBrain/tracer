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
        private JToken SaveMethodTracer(MethodTracer methodTracer)
        {
            var savedTracedMetod = new JObject
            {
                { "name", methodTracer.MethodName },
                { "class", methodTracer.ClassName },
                { "time", methodTracer.ElapsedTime.Milliseconds + "ms" }
            };

            if (methodTracer.Inner.Any())
                savedTracedMetod.Add("methods", new JArray(from mt in methodTracer.Inner
                                                           select SaveMethodTracer(mt)));
            return savedTracedMetod;
        }

        private JToken SaveThreadTracer(ThreadTracer threadTracer)
        {
            var extremeMethods = from method in threadTracer.ExtremeMethods
                                 select SaveMethodTracer(method);
            return new JObject
            {
                { "id", threadTracer.ThreadId },
                { "time", threadTracer.TimeElapsed.Milliseconds + "ms"},
                { "methods", new JArray(extremeMethods) }
            };
        }

        override public void SaveTraceResult(TextWriter textWriter, TraceResult traceResult)
        {
            var sortedThreadTracers = from item in traceResult.ThreadTracers
                                      orderby item.Key
                                      select item.Value;
            var jtokens = from threadTracer in sortedThreadTracers
                     select SaveThreadTracer(threadTracer);
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
