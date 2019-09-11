using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace;
using Trace.Saver;
using System.Threading;
using System.IO;

namespace TracerUseExample
{
    class Program
    {
        static private Tracer _tracer = new Tracer();
        static private Saver _saver = new XMLSaver();

        static void DoIT()
        {
            _tracer.StartTrace();
            m0();
            m1();
            _tracer.StartTrace();
            _tracer.StopTrace();
            _tracer.StopTrace();
        }

        static void m0()
        {
            _tracer.StartTrace();
            Thread.Sleep(100);
            m1();
            _tracer.StartTrace();
            Thread.Sleep(15);
            _tracer.StopTrace();
            _tracer.StopTrace();
        }

        static void m1()
        {
            _tracer.StartTrace();
            Thread.Sleep(100);
            _tracer.StopTrace();
        }

        static void Main(string[] args)
        {
            Thread thread1 = new Thread(new ThreadStart(DoIT));
            Thread thread2 = new Thread(new ThreadStart(DoIT));
            thread1.Name = "FIRST";
            thread2.Name = "SECOND";
            thread1.Start();
            Thread.Sleep(100);
            thread2.Start();
            thread1.Join(); // wait all threads terminate
            thread2.Join();
            TraceResult traceResult = _tracer.GetTraceResult();
            FileStream fileStream = new FileStream("test.xml", FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            _saver.SaveTraceResult(streamWriter, traceResult);
            _saver.SaveTraceResult(Console.Out, traceResult);
            _saver = new JSONSaver();
            _saver.SaveTraceResult(Console.Out, traceResult);
            fileStream = new FileStream("test.json", FileMode.Create);
            streamWriter = new StreamWriter(fileStream);
            _saver.SaveTraceResult(streamWriter, traceResult);
        }
    }
}
