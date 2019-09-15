using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trace
{
    public class TraceResult
    {
        public IDictionary<int, ThreadTracerResult> ThreadTracerResults { get; }

        public TraceResult(ConcurrentDictionary<int, ThreadTracer> threadTracers)
        {
            ThreadTracerResults = new Dictionary<int, ThreadTracerResult>();
            foreach (var threadTracer in threadTracers)
            {
                ThreadTracerResults[threadTracer.Key] = ThreadTracerResult.GetResult(threadTracer.Value);
            }
        }
    }
}
