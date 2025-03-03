using System;
using System.Collections;


namespace Collection
{
   public  class PArrayList
    {

        public static void Run()
        {

            ArrayList list = new ArrayList();
            //add
            list.Add(1);
            list.Add(2);
            list.Add(3);

            //insert
            list.Insert(0, 0);
            //remove
            list.Remove(1);

            //remove at
            list.RemoveAt(0);

            foreach (int item in list)
            {
               Console.WriteLine(item);
            }

            list.Add(1);
            list.Insert(0, 0);


            //sort
            list.Sort();
            foreach (int item in list)
            {
                Console.WriteLine(item);
            }

            //Reverse
            list.Reverse();
            foreach (int item in list)
            {
                Console.WriteLine(item);
            }

            list.IndexOf(2);
          
        }
    }
}