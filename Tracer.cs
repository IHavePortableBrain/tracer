using System;
using System.Collections.Generic;
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
        private MethodTracer _methodTracer;

        public Tracer()
        {
            _traceResult = new TraceResult();
        }

        public void StartTrace()
        {
            MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
            MethodTracer methodTracer = new MethodTracer(methodBase.ReflectedType.Name, methodBase.Name);
            _methodTracer = methodTracer;
            _methodTracer.StartTrace();
            //ThreadResult curThreadResult = _traceResult.AddOrGetThreadResult(Thread.CurrentThread.ManagedThreadId);
            //curThreadResult.StartTracingMethod(methodResult);
        }

        public void StopTrace()
        {
            _methodTracer.StopTrace();
            //int threadId = Thread.CurrentThread.ManagedThreadId;
            //try
            //{
            //    _traceResult.GetThreadResult(threadId).StopTracingMethod();
            //}
            //catch (KeyNotFoundException e)
            //{
            //    throw new KeyNotFoundException("Thread #" + threadId.ToString() + " was not traced.");
            //}
        }

        public TraceResult GetTraceResult()
        {
            return new TraceResult(_methodTracer.ElapsedTime);
        }
    }
}
