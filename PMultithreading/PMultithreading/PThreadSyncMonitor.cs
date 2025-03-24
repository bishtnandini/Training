using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PMultithreading
{
    class PThreadSyncMonitor
    {
        private static object _locker = new object();

        public static void run()
        {
            for (int i = 0; i < 3; i++)
            {
                new Thread(Dowork).Start();
            }
        }

        public static void Dowork()
        {
            try { 
               Monitor.Enter(_locker);
           
                Console.WriteLine($"thread {Thread.CurrentThread.ManagedThreadId} starting...");
                Thread.Sleep(2000);
                throw new Exception();
                //Console.WriteLine($"thread {Thread.CurrentThread.ManagedThreadId} completed...");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Monitor.Exit(_locker);
            }
        }
    }
}
