using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection_Generic
{
    public class PHashTable
    {
        //similar to a Dictionary, but it is not type-safe since it stores objects.
        public static void Run()
        {
            Hashtable ht = new Hashtable();

            //add
            ht.Add(1, "hi");
            ht.Add(2, "hello");
            ht.Add(3, "hey");

            //remove
            ht.Remove(2);

            //contain
            if (ht.ContainsKey(3))
            {
                Console.WriteLine("Key 3 exists in the Hashtable.");
            }
            else
            {
                Console.WriteLine("Key 3 does not exist.");
            }

            //clone
            Hashtable htt = (Hashtable)ht.Clone();
            htt.Add(4, "yo");
            foreach (DictionaryEntry item in htt)
            {
                Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            }

          //keys
            foreach (var key in htt.Keys)
            {
                Console.WriteLine(key);
            }

            //values
            foreach (var val in htt.Values)
            {
                Console.WriteLine(val);
            }

            //both

            foreach (DictionaryEntry i in htt)
            {
                Console.WriteLine($"key is {i.Key} , value is {i.Value}");
            }



        }
    }
}
