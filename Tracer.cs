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
        private Stack<MethodTracer> _unstopped;
        private MethodTracer _lastStopped;

        public Tracer()
        {
            _traceResult = null;
            _unstopped = new Stack<MethodTracer>();
            _threadTracers = new ConcurrentDictionary<int, ThreadTracer>();
        }

        public void StartTrace()
        {
            MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
            MethodTracer methodTracer = new MethodTracer(methodBase.ReflectedType.Name, methodBase.Name);
            StartTraceMethod(methodTracer);
            //ThreadResult curThreadResult = _traceResult.AddOrGetThreadResult(Thread.CurrentThread.ManagedThreadId);
            //curThreadResult.StartTracingMethod(methodResult);
        }

        public void StopTrace()
        {
            _lastStopped = _unstopped.Pop(); //rewrite with unstopped.peek().StopTrace() ?
            _lastStopped.StopTrace();
        }

        private void StartTraceMethod(MethodTracer methodTracer)
        {
            if (_unstopped.Count > 0)
            {
                MethodTracer lastUnstoppedMethodTracer = _unstopped.Peek();
                lastUnstoppedMethodTracer.AddInner(methodTracer);
            }
            methodTracer.StartTrace();
            _unstopped.Push(methodTracer);
        }

        public TraceResult GetTraceResult()
        {
            return new TraceResult(_lastStopped, _lastStopped.ElapsedTime);
        }
    }
}
