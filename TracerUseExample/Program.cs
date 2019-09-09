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
            _tracer.StartTrace();
            m0();
            _tracer.StopTrace();
            Console.WriteLine(_tracer.GetTraceResult().TimeSpan);
            Console.ReadKey();
        }
    }
}
