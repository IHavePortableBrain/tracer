using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trace
{
    public class TraceResult
    {
        public TimeSpan TimeSpan;
        public MethodTracer LastStoppedMethodTracer;

        public TraceResult(MethodTracer methodTracer, TimeSpan timeSpan = new TimeSpan())
        {
            LastStoppedMethodTracer = methodTracer;
            TimeSpan = timeSpan;
        }
    }
}
