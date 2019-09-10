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
        public TimeSpan TimeSpan;
        public ConcurrentDictionary<int, ThreadTracer> ThreadTracers;

        public TraceResult(ConcurrentDictionary<int, ThreadTracer> threadTracers, TimeSpan timeSpan = new TimeSpan())
        {
            ThreadTracers = threadTracers;
            TimeSpan = timeSpan;
        }
    }
}
