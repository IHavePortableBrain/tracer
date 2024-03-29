﻿using System;
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
        private readonly ConcurrentDictionary<int, ThreadTracer> _threadTracers;
        static private object _locker = new object();

        public Tracer()
        {
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
                if (!_threadTracers.TryGetValue(id, out ThreadTracer threadTracer))
                {
                    threadTracer = new ThreadTracer(id);
                    _threadTracers[id] = threadTracer;
                }
                return threadTracer;
            }
        }

        public TraceResult GetTraceResult()
        {
            _traceResult = new TraceResult(_threadTracers);
            return _traceResult;
        }
    }
}
