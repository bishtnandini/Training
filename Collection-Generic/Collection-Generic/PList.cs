using System;
using System.Collections.Generic;

namespace Collection_Generic
{
    public class PList
    {
        //similar to arraylist but more type secure, better performance
        public static void Run()
        {
            List<int> li = new List<int>();

            // Add elements to the integer list
            li.Add(1);
            li.Add(2);

            // List of Customer objects 
            List<Customer> cus = new List<Customer>();

            //if name is property
            //cus.Add(new Customer { name = "Nandini" });

            //if name is variable field 
            cus.Add(new Customer("Nandini"));

            // Display customers
            foreach (var c in cus)
            {
                Console.WriteLine("Customer Name: " + c.name);
            }

            var states = new[]
            {
              "Uttarakhand",
              "A&N",
              "J&K"
            };

            List<string> st = new List<string>(states);
            st.Add("himachal pradesh");

            st.ForEach(x => Console.WriteLine(x));

            //addrange (copying list)
            List<string> newst = new List<string>();
            newst.Add("Punjab");
            newst.AddRange(st);
            foreach (var c in newst)
            {
                Console.WriteLine( c);
            }

            //insert
            newst.Insert(1, "Washington");

            //remove
            newst.Remove("Washington");
            //removaALL
            newst.RemoveAll( x => x == "Washington");

            //contains
            Console.WriteLine(newst.Contains("Uttarakhand"));


            //indexof
            Console.WriteLine(newst.IndexOf("Uttarakhand"));

            //find
            Console.WriteLine(newst.Find(x=>x.Contains("&")));

            //findALL
          var result=newst.FindAll(x => x.Contains("&"));
            foreach (var item in result)
            {
                Console.WriteLine(item);                
            }
        }
    }

    public class Customer
    {
       // public string name { get; set; }
        
        public string name;
        public Customer(string name)
        {
            this.name = name;
        }
        
    }
}
