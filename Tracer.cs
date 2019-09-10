using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Trace
{
    public class Tracer:ITracer
    {
        private TraceResult _traceResult;
        private ConcurrentDictionary<int, ThreadTracer> _threadTracers;
        static private object _locker = new object();


        public Tracer()
        {
            _traceResult = null;
            _threadTracers = new ConcurrentDictionary<int, ThreadTracer>();
        }

        public void StartTrace()
        {
            MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
            MethodTracer methodTracer = new MethodTracer(methodBase.ReflectedType.Name, methodBase.Name);
            ThreadTracer curThreadTracer = AddOrGetThreadTracer(Thread.CurrentThread.ManagedThreadId);
            curThreadTracer.StartTraceMethod(methodTracer);
        }

        public void StopTrace()
        {
            AddOrGetThreadTracer(Thread.CurrentThread.ManagedThreadId).StopTraceMethod();
        }

        private ThreadTracer AddOrGetThreadTracer(int id)
        {
            lock (_locker)
            {
                ThreadTracer threadTracer;
                if (!_threadTracers.TryGetValue(id, out threadTracer))
                {
                    threadTracer = new ThreadTracer();
                    _threadTracers[id] = threadTracer;
                }
                return threadTracer;
            }
        }

        public TraceResult GetTraceResult()
        {
            return new TraceResult(_threadTracers);//_lastStopped, _lastStopped.ElapsedTime
        }
    }
}
