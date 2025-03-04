using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection_Generic
{
   public  class PSortedList
    {
        //sortedlist and sorted dict are very similar but for memnory and efficency we prefer sl
        public static void Run()
        {
            SortedList<string, string> sl = new SortedList<string, string>();
            sl.Add("hi", "hello");
            sl.Add("a", "aaaa");
            sl.Add("b", "bb");
            sl["c"] = "ccc";

            //sorted order data is saved auto.
            foreach (var i in sl)
            {
                Console.WriteLine($"the key is {i.Key} and the value is {i.Value}");
            }

            //remove
            sl.Remove("hi");
            //contain
            Console.WriteLine(sl.ContainsKey("hi"));
            Console.WriteLine(sl.ContainsKey("a"));


            sl.RemoveAt(1);

            foreach (var k in sl.Keys)
            {
                Console.WriteLine(k);


            }
        }   }
}
