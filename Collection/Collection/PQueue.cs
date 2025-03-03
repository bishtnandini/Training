using System;
using System.Collections;

namespace Collection
{
   public  class PQueue
    {
        public static void Run()
        {
            //FIFO
            Queue qu = new Queue();
            qu.Enqueue(1);
            qu.Enqueue(2);
            qu.Enqueue(3);
            qu.Enqueue(4);

            //pop
            qu.Dequeue();

            //peek
            Console.WriteLine(qu.Peek());

            foreach (var item in qu)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine(qu.Contains(1));
        }
    }
}
