using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PMultithreading
{
  
    class Pmulti
    {
        public static void thrd1()
        {
            for (int i = 0; i <= 10; i++)
            {
                Console.WriteLine($"thread1 is runing for  thread1 {i}");
                if (i == 5)
                {
                    Thread.Sleep(5000);
                }
            }
        }

        public static void thrd2()
        {
            for (int i = 0; i <= 10; i++)
            {
                Console.WriteLine($"thread1 is runing for thread2 {i}");

            }
        }
        public static void run()
        {
            Thread t1 = new Thread(thrd1);
            Thread t2 = new Thread(thrd2);
            t1.Start();
            t2.Start();


        }
        
    }
}
