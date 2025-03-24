using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PMultithreading
{
    class PThreadSyncSemaphore
    {
        static Semaphore _semaphore = new Semaphore(2, 2);

        public static void run()
        {
            for(int i = 0; i < 5; i++)
            {
                new Thread(write).Start();
            }
        }

        public static void write()
        {
            Console.WriteLine($"thread {Thread.CurrentThread.ManagedThreadId} is waiting");
            _semaphore.WaitOne();
            Thread.Sleep(2000);
            Console.WriteLine($"thread {Thread.CurrentThread.ManagedThreadId} is writing..");
            Thread.Sleep(2000);
            Console.WriteLine($"thread {Thread.CurrentThread.ManagedThreadId} is writing completed..");
            _semaphore.Release();
        }
    }
}
