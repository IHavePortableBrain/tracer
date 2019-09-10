using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Trace
{
    public class ThreadTracer
    {
        private Stack<MethodTracer> _unstopped;
        private MethodTracer _lastStopped;

        public ThreadTracer()
        {
            _unstopped = new Stack<MethodTracer>();
        }

        internal void StartTraceMethod(MethodTracer methodTracer)
        {
            if (_unstopped.Count > 0)
            {
                MethodTracer lastUnstoppedMethodTracer = _unstopped.Peek();
                lastUnstoppedMethodTracer.AddInner(methodTracer);
            }
            methodTracer.StartTrace();
            _unstopped.Push(methodTracer);
        }

        internal void StopTraceMethod()
        {
            _lastStopped = _unstopped.Pop(); //rewrite with unstopped.peek().StopTrace() ? 
            _lastStopped.StopTrace();
            //Console.WriteLine("thread " + Thread.CurrentThread.Name + " stops tracing " + _lastStopped.MethodName);
        }
    }
}
