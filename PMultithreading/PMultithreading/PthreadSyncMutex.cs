using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PMultithreading
{
    class PthreadSyncMutex
    {
        static Mutex _mutex = new Mutex();

        public static void run()
        {
            for(int i = 0; i < 3; i++)
            {
                new Thread(write).Start();
            }
        }

        public static void write()
        {
            Console.WriteLine($"thread {Thread.CurrentThread.ManagedThreadId} is waiting");
            _mutex.WaitOne();
            Thread.Sleep(2000);
            Console.WriteLine($"thread {Thread.CurrentThread.ManagedThreadId} is writing..");
            Thread.Sleep(2000);
            Console.WriteLine($"thread {Thread.CurrentThread.ManagedThreadId} is writing completed..");
            _mutex.ReleaseMutex();

        }
    }
}
