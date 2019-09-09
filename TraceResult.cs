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
        public TraceResult(TimeSpan timeSpan = new TimeSpan())
        {
            TimeSpan = timeSpan;
        }
    }
}
