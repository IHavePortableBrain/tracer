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
        static void Main(string[] args)
        {
            Tracer tracer = new Tracer();
            tracer.StartTrace();

            tracer.StartTrace();
            Thread.Sleep(100);
            tracer.StopTrace();

            tracer.StartTrace();
            Thread.Sleep(100);
            tracer.StartTrace();
            Thread.Sleep(200);
            tracer.StopTrace();
            tracer.StartTrace();
            tracer.StartTrace();
            Thread.Sleep(200);
            tracer.StopTrace();
            tracer.StopTrace();
            tracer.StopTrace();

            tracer.StopTrace();
            Console.WriteLine(tracer.GetTraceResult().TimeSpan);
            Console.ReadKey();
        }
    }
}
