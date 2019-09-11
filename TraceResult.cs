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
        public ConcurrentDictionary<int, ThreadTracer> ThreadTracers { get; }

        public TraceResult(ConcurrentDictionary<int, ThreadTracer> threadTracers)
        {
            ThreadTracers = threadTracers;
        }
    }
}
