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
        public List<MethodTracer> Inner;
        private Stopwatch _stopwatch;
        

        public MethodTracer(string className, string methodName)
        {
            ClassName = className;
            MethodName = methodName;
            _stopwatch = new Stopwatch();
            Inner = new List<MethodTracer>();
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
            Inner.Add(toAdd);
        }
    }
}
