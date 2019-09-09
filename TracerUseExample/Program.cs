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
            Thread.Sleep(300);
            tracer.StopTrace();
            Console.WriteLine(tracer.GetTraceResult().TimeSpan);
            Console.ReadKey();
        }
    }
}
