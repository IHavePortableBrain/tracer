using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    class MethodResult
    {
        public string ClassName { get; protected set; }
        public string MethodName { get; protected set; }

        public TimeSpan ElapsedTime
        {
            get { return _stopwatch.Elapsed; }
        }

        private Stopwatch _stopwatch;

        public MethodResult(string className, string methodName)
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
