using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Tracer
{
    class Tracer:ITracer
    {
        private TraceResult _traceResult;

        public Tracer()
        {
            _traceResult = new TraceResult();
        }

        public void StartTrace()
        {
            MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
            MethodResult methodResult = new MethodResult(methodBase.ReflectedType.Name, methodBase.Name);
            //ThreadResult curThreadResult = _traceResult.AddOrGetThreadResult(Thread.CurrentThread.ManagedThreadId);
            //curThreadResult.StartTracingMethod(methodResult);
        }

        public void StopTrace()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            try
            {
                _traceResult.GetThreadResult(threadId).StopTracingMethod();
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException("Thread #" + threadId.ToString() + " was not traced.");
            }
        }

        public TraceResult GetTraceResult()
        {
            return _traceResult;
        }
    }
}
