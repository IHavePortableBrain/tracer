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
            ThreadTracerResult[] threadTracerResult = new ThreadTracerResult[_tracer.GetTraceResult().ThreadTracerResults.Count];
            //_tracer.GetTraceResult().ThreadTracers.ToList()
            _tracer.GetTraceResult().ThreadTracerResults.Values.CopyTo(threadTracerResult, 0);
            Assert.AreEqual(threadTracerResult[0].ExtremeMethodResults.Count, 2);
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
            ThreadTracerResult[] threadTracersResults = new ThreadTracerResult[_tracer.GetTraceResult().ThreadTracerResults.Count];
            _tracer.GetTraceResult().ThreadTracerResults.Values.CopyTo(threadTracersResults, 0);
            long actual = threadTracersResults[0].TimeElapsed.Milliseconds;
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
            foreach (KeyValuePair<int, ThreadTracerResult> keyValuePair in _tracer.GetTraceResult().ThreadTracerResults)
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
            ThreadTracerResult[] threadTracerResults = new ThreadTracerResult[traceResult.ThreadTracerResults.Count];
            traceResult.ThreadTracerResults.Values.CopyTo(threadTracerResults, 0);

            Assert.AreEqual(1, traceResult.ThreadTracerResults.Count);
            MethodTracerResult extremeMt = threadTracerResults[0].ExtremeMethodResults[0];
            Assert.AreEqual(nameof(TestTracer), extremeMt.ClassName); 
            Assert.AreEqual(nameof(InnerMethodTest), extremeMt.MethodName);
            TestIsGreater(extremeMt.ElapsedTime.Milliseconds, _waitTime * 2);
            Assert.AreEqual(1, extremeMt.Inner.Count);
            MethodTracerResult internalMt = extremeMt.Inner[0];
            Assert.AreEqual(nameof(TestTracer), internalMt.ClassName);
            Assert.AreEqual(nameof(SingleThreadedMethod), internalMt.MethodName);
            TestIsGreater(internalMt.ElapsedTime.Milliseconds, _waitTime);
        }
    }
}

