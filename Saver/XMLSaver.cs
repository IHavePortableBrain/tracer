using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;


namespace Trace.Saver
{
    public class XMLSaver:Saver
    {
        private XElement SaveMethodTracer(MethodTracer methodTracer)
        {
            var savedTracedMetod = new XElement("method",
                new XAttribute("name", methodTracer.MethodName),
                new XAttribute("time", methodTracer.ElapsedTime.Milliseconds + "ms"),
                new XAttribute("class", methodTracer.ClassName));

            if (methodTracer.Inner.Any())
                savedTracedMetod.Add(from innerMethod in methodTracer.Inner
                                          select SaveMethodTracer(innerMethod));
            return savedTracedMetod;
        }

        private XElement SaveThreadTracer(ThreadTracer threadTracer)
        {
            return new XElement("thread",
                new XAttribute("id", threadTracer.ThreadId),
                new XAttribute("time", threadTracer.LastStopped.ElapsedTime.Milliseconds + "ms"),
                SaveMethodTracer(threadTracer.LastStopped)
                );
        }

        override public void SaveTraceResult(TextWriter textWriter, TraceResult traceResult)
        {
            var sortedThreadTracers = from item in traceResult.ThreadTracers
                                      orderby item.Key
                                      select item.Value;

            XDocument doc = new XDocument(
                new XElement("root", from threadTracer in sortedThreadTracers
                                     select SaveThreadTracer(threadTracer)
                ));

            using (XmlTextWriter xmlWriter = new XmlTextWriter(textWriter))
            {
                xmlWriter.Formatting = Formatting.Indented;
                doc.WriteTo(xmlWriter);
            }
        }
    }
}
