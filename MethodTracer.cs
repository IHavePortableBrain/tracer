using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trace
{
    public class MethodTracer
    {
        public string ClassName { get; protected set; }
        public string MethodName { get; protected set; }
        public TimeSpan ElapsedTime
        {
            get { return _stopwatch.Elapsed; }
        }

        private Stopwatch _stopwatch;

        public MethodTracer(string className, string methodName)
        {
            ClassName = className;
            MethodName = methodName;
            _stopwatch = new Stopwatch();
        }

        public void StartTrace()
        {
            _stopwatch.Restart();
        }

        public void StopTrace()
        {
            _stopwatch.Stop();
        }
    }
}
