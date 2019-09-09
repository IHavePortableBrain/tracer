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
        private List<MethodTracer> _inner;

        public MethodTracer(string className, string methodName)
        {
            ClassName = className;
            MethodName = methodName;
            _stopwatch = new Stopwatch();
            _inner = new List<MethodTracer>();
        }

        public void StartTrace()
        {
            //if (!_isTraceStarted)
            //{
                //method tracing starts, no inner methods traced
                //_isTraceStarted = true;
                _stopwatch.Start();
            //}
            //else
            //{
            //    //method tracing countinue, add inner method tracing
            //}
        }

        public void StopTrace()
        {
            _stopwatch.Stop();
        }

        public void AddInner(MethodTracer toAdd)
        {
            _inner.Add(toAdd);
        }
    }
}
