using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trace;
using System.Threading;
using System.Collections.Generic;
using System;

namespace TestTracer
{
    [TestClass]
    public class TestTracer
    {
        private readonly Tracer _tracer;
        private const int _waitTime = 100;
        private const int _threadCount = 4;

        public TestTracer()
        {
            _tracer = new Tracer();
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

        private void MultiThreadedMethod()
        {
            var threads = new List<Thread>();
            Thread newThread;
            for (int i = 0; i < _threadCount; i++)
            {
                newThread = new Thread(SingleThreadedMethod);
                threads.Add(newThread);
            }
            foreach (Thread thread in threads)
            {
                thread.Start();
            }
            _tracer.StartTrace();
            Thread.Sleep(_waitTime);
            _tracer.StopTrace();
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }

        private void CorruptedMethod()
        {
            Thread.Sleep(100);
            _tracer.StopTrace();
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
            _tracer.StartTrace();
            Thread.Sleep(_waitTime);
            _tracer.StopTrace();
            ThreadTracer[] threadTracers = new ThreadTracer[_tracer.GetTraceResult().ThreadTracers.Count];
            _tracer.GetTraceResult().ThreadTracers.Values.CopyTo(threadTracers, 0);
            long actual = threadTracers[0].LastStopped.ElapsedTime.Milliseconds;
            TestIsGreater(actual, _waitTime);
        }

        [TestMethod]
        public void MultiThreadTest()
        {
            // only checks time
            var threads = new List<Thread>();
            long expected = 0;
            Thread newThread;
            for (int i = 0; i < _threadCount; i++)
            {
                newThread = new Thread(SingleThreadedMethod);
                threads.Add(newThread);
                newThread.Start();
                expected += _waitTime;
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            long actual = 0;
            foreach (KeyValuePair<int, ThreadTracer> keyValuePair in _tracer.GetTraceResult().ThreadTracers)
            {
                actual += keyValuePair.Value.LastStopped.ElapsedTime.Milliseconds;
            }
            TestIsGreater(actual, expected);
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
            TestIsGreater(threadTracers[0].LastStopped.ElapsedTime.Milliseconds, _waitTime * 2);
            Assert.AreEqual(1, threadTracers[0].LastStopped.Inner.Count);
            MethodTracer methodTracer = threadTracers[0].LastStopped;
            Assert.AreEqual(nameof(TestTracer), methodTracer.ClassName); 
            Assert.AreEqual(nameof(InnerMethodTest), methodTracer.MethodName);
            TestIsGreater(methodTracer.ElapsedTime.Milliseconds, _waitTime * 2);
            Assert.AreEqual(1, methodTracer.Inner.Count);
            MethodTracer innerMethodResult = methodTracer.Inner[0];
            Assert.AreEqual(nameof(TestTracer), innerMethodResult.ClassName);
            Assert.AreEqual(nameof(SingleThreadedMethod), innerMethodResult.MethodName);
            TestIsGreater(innerMethodResult.ElapsedTime.Milliseconds, _waitTime);
        }
    }
}

