using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trace;
using System.Threading;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Trace.Test
{
    [TestClass]
    public class TestTracer
    {
        private readonly Tracer _tracer;
        private const int _waitTime = 100;
        private const int _threadCount = 4;

        public TestTracer()
        {
            _tracer = new Trace.Tracer();
        }

        private void TestIsGreater(long greater, long less)
        {
            Assert.IsTrue(greater >= less);
        }

        private void SingleThreadedMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(_waitTime);
            _tracer.StopTrace();
        }

        private void CorruptedMethod()
        {
            Thread.Sleep(_waitTime);
            _tracer.StopTrace();
        }
        
        private void DoubleExtremeMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(_waitTime);
            _tracer.StopTrace();
            _tracer.StartTrace();
            Thread.Sleep(_waitTime);
            _tracer.StopTrace();
        }

        [TestMethod]
        public void DoubleExtremeMethodTest()
        {
            DoubleExtremeMethod();
            ThreadTracer[] threadTracers = new ThreadTracer[_tracer.GetTraceResult().ThreadTracers.Count];
            //_tracer.GetTraceResult().ThreadTracers.ToList()
            _tracer.GetTraceResult().ThreadTracers.Values.CopyTo(threadTracers, 0);
            Assert.AreEqual(threadTracers[0].ExtremeMethods.Count, 2);
        }

        [TestMethod]
        public void CorruptionTest()
        {
            Action action = CorruptedMethod;
            Assert.ThrowsException<InvalidOperationException>(action);
        }

        [TestMethod]
        public void SingleThreadTest()
        {
            // only checks time
            SingleThreadedMethod();
            ThreadTracer[] threadTracers = new ThreadTracer[_tracer.GetTraceResult().ThreadTracers.Count];
            _tracer.GetTraceResult().ThreadTracers.Values.CopyTo(threadTracers, 0);
            long actual = threadTracers[0].TimeElapsed.Milliseconds;
            TestIsGreater(actual, _waitTime);
        }

        [TestMethod]
        public void MultiThreadTest()
        {
            // only checks time
            var threads = new List<Thread>();
            long eachThreadElapsedSum = 0;
            Thread newThread;
            for (int i = 0; i < _threadCount; i++)
            {
                newThread = new Thread(SingleThreadedMethod);
                threads.Add(newThread);
                newThread.Start();
                eachThreadElapsedSum += _waitTime;
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            long actual = 0;
            foreach (KeyValuePair<int, ThreadTracer> keyValuePair in _tracer.GetTraceResult().ThreadTracers)
            {
                actual += keyValuePair.Value.TimeElapsed.Milliseconds;
            }
            TestIsGreater(actual, eachThreadElapsedSum);
        }

        [TestMethod]
        public void InnerMethodTest()
        {
            // checks time, amount, classnames and methodnames 
            _tracer.StartTrace();
            Thread.Sleep(_waitTime);
            SingleThreadedMethod();
            _tracer.StopTrace();
            TraceResult traceResult = _tracer.GetTraceResult();
            ThreadTracer[] threadTracers = new ThreadTracer[traceResult.ThreadTracers.Count];
            traceResult.ThreadTracers.Values.CopyTo(threadTracers, 0);

            Assert.AreEqual(1, traceResult.ThreadTracers.Count);
            MethodTracer extremeMt = threadTracers[0].ExtremeMethods[0];
            Assert.AreEqual(nameof(TestTracer), extremeMt.ClassName); 
            Assert.AreEqual(nameof(InnerMethodTest), extremeMt.MethodName);
            TestIsGreater(extremeMt.ElapsedTime.Milliseconds, _waitTime * 2);
            Assert.AreEqual(1, extremeMt.Inner.Count);
            MethodTracer internalMt = extremeMt.Inner[0];
            Assert.AreEqual(nameof(TestTracer), internalMt.ClassName);
            Assert.AreEqual(nameof(SingleThreadedMethod), internalMt.MethodName);
            TestIsGreater(internalMt.ElapsedTime.Milliseconds, _waitTime);
        }
    }
}

