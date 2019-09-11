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
        public MethodTracer LastStopped { get; private set; }
        public int ThreadId { get; private set; }

        private Stack<MethodTracer> _unstopped;

        public ThreadTracer(int id)
        {
            _unstopped = new Stack<MethodTracer>();
            ThreadId = id;
        }

        internal void StartTraceMethod(MethodTracer methodTracer)
        {
            if (_unstopped.Count > 0)
            {
                MethodTracer lastUnstoppedMethodTracer = _unstopped.Peek();
                lastUnstoppedMethodTracer.Inner.Add(methodTracer);
            }
            methodTracer.StartTrace();
            _unstopped.Push(methodTracer);
        }

        internal void StopTraceMethod()
        {
            LastStopped = _unstopped.Pop(); //rewrite with unstopped.peek().StopTrace() ? 
            LastStopped.StopTrace();
            //Console.WriteLine("thread " + Thread.CurrentThread.Name + " stops tracing " + _lastStopped.MethodName);
        }
    }
}
