using System;
using System.Collections;
using System.Collections.Generic;


namespace Collection
{
   public class PStack
    {
        public static void Run()
        {
            //LIFO

            Stack st = new Stack();
            st.Push(1);
            st.Push(2);
            st.Push(3);
            st.Push(4);
           

            //pop
            st.Pop();

            //peek
            Console.WriteLine(st.Peek());

           

            Console.WriteLine( st.Contains(1));

            int[] arr = new int[] { 9, 8, 7, 6, 5 };
            st.CopyTo(arr, 1);
            foreach (var item in arr)
            {
                Console.WriteLine(item);
            }
        }
    }
}
