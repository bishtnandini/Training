using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection_Generic
{
    public class PDictionary
    {
        public static void Run()
        {
            //store data in key value pair and key must be unique and case sensitive (cs)

            Dictionary<string, string> dict = new Dictionary<string, string>();
            Dictionary<int, string> dict1 = new Dictionary<int, string>
            {
                {1,"jan"},
                {2,"feb"}
            };
            //add
            dict1[101] = "nandini";
            dict1.Add(102, "hello");

            dict.Add("sun", "sunday");
            dict.Add("Sun", "sunday"); //will not show error because of cs

            //case insensitive
            Dictionary<string, string> dict2 = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            //trygetvalue if key exists then return value otherwise null
            Console.WriteLine(dict.TryGetValue("sun", out string sun)); // sun has sunday store and it return true

            //remove
            dict.Remove("sun");

            //clear dict
            //dict.Clear();

            foreach(var key in dict.Keys)
            {
                Console.WriteLine(key);
            }

            foreach (var val in dict.Values)
            {
                Console.WriteLine(val);
            }

            foreach (var i in dict)
            {
                Console.WriteLine($"key is {i.Key} , value is {i.Value}");
            }
        }
    }
}
