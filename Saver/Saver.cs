using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Trace.Saver
{
    public abstract class Saver:ISaveTraceResult
    {
        public abstract void SaveTraceResult(TextWriter textWriter, TraceResult traceResult);
    }
}
