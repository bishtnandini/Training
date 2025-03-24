using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PMultithreading
{
    public class mainthread1
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


    }

    class PsingleThread
    {

         public static void run()
        {
            //singlr thread example
            mainthread1.thrd1();
            mainthread1.thrd2();




        }
    }
}
