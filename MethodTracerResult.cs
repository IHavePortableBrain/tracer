using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trace
{
    public class MethodTracerResult
    {
        public string ClassName { get; protected set; }
        public string MethodName { get; protected set; }
        public TimeSpan ElapsedTime { get; protected set; }
        public List<MethodTracerResult> Inner;

        static internal MethodTracerResult GetResult(MethodTracer methodTracer)
        {
            MethodTracerResult result = new MethodTracerResult();
            result.ClassName = methodTracer.ClassName;
            result.MethodName = methodTracer.MethodName;
            result.ElapsedTime = methodTracer.ElapsedTime;
            result.Inner = new List<MethodTracerResult>();
            foreach (var innerMethodTracer in methodTracer.Inner)
            {
                result.Inner.Add(MethodTracerResult.GetResult(innerMethodTracer));
            }

            return result;
        }

    }
}
