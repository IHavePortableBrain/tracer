using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trace
{
    public class ThreadTracerResult
    {
        public IList<MethodTracerResult> ExtremeMethodResults { get; private set; }
        public int ThreadId { get; private set; }
        public TimeSpan TimeElapsed { get; private set; }

        static internal ThreadTracerResult GetResult(ThreadTracer threadTracer)
        {
            ThreadTracerResult result = new ThreadTracerResult();
            result.ExtremeMethodResults = new List<MethodTracerResult>();
            foreach (var extremeMethod in threadTracer.ExtremeMethods)
            {
                result.ExtremeMethodResults.Add(MethodTracerResult.GetResult(extremeMethod));
            }
            result.ThreadId = threadTracer.ThreadId;
            result.TimeElapsed = threadTracer.TimeElapsed;
            return result;
        }
    }
}
