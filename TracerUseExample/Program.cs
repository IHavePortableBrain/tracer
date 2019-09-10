using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace;
using System.Threading;

namespace TracerUseExample
{
    class Program
    {
        static private Tracer _tracer;

        static void DoIT()
        {
            _tracer.StartTrace();
            m0();
            _tracer.StartTrace();
            _tracer.StopTrace();
            _tracer.StopTrace();
        }

        static void m0()
        {
            _tracer.StartTrace();
            Thread.Sleep(100);
            m1();
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
            _tracer = new Tracer();
            Thread thread1 = new Thread(new ThreadStart(DoIT));
            Thread thread2 = new Thread(new ThreadStart(DoIT));
            thread1.Name = "FIRST";
            thread2.Name = "SECOND";
            thread1.Start();
            Thread.Sleep(100);
            thread2.Start();
            thread1.Join(); // wait all threads terminate
            thread2.Join();
            Console.WriteLine(_tracer.GetTraceResult().TimeSpan);
        }
    }
}
