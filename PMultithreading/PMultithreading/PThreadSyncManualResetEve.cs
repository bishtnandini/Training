using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PMultithreading
{
    class PThreadSyncManualResetEve
    {
        //we use this when we want one thread doing its tasl all thread wait till that thread complete that task 
        //Reader Writer Problem

        static ManualResetEvent _mre = new ManualResetEvent(false);

        public static void run()
        {
            new Thread(write).Start();
            for(int i = 0; i < 3; i++)
            {
                new Thread(read).Start();
            }

        }

        public static void write()
        {
            Console.WriteLine($"thread {Thread.CurrentThread.ManagedThreadId} writing");
            _mre.Reset();
            Thread.Sleep(2000);
            Console.WriteLine($"thread {Thread.CurrentThread.ManagedThreadId} writing completed");
            _mre.Set();

        }

        public static void read()
        {
            Console.WriteLine($"thread {Thread.CurrentThread.ManagedThreadId} waiting");
            _mre.WaitOne();
           
            Console.WriteLine($"thread {Thread.CurrentThread.ManagedThreadId} reading");
           

        }
    }
}
