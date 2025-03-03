using System;
using System.Collections;

namespace Collection
{
   public class PArray
    {
        public static void Run()
        {

            int[] arr = new int[5];
            int[] arr1 = new int[] { 1, 2, 3, 4, 5, 6 };
            string[] cars = new string[4] { "Porsche", "BMW", "Jaguar", "Bentley" };

            for(int i =0; i < cars.Length; i++)
            {
                Console.WriteLine(cars[i]);
            }

            Console.WriteLine("after sorting");

            //Sort
            System.Array.Sort(cars);
            for (int i = 0; i < cars.Length; i++)
            {
                Console.WriteLine(cars[i]);
            }

            //Reverse
            System.Array.Reverse(cars);
            for (int i = 0; i < cars.Length; i++)
            {
                Console.WriteLine(cars[i]);
            }

            //Rank (dimension of array) 
            Console.WriteLine("Rank of oneDArray: " + cars.Rank);

            //Indexof
            Console.WriteLine(System.Array.IndexOf(cars, "BMW"));

            //exists

            Console.WriteLine(System.Array.Exists(cars, car => car == "BMW"));

            //Find
            Console.WriteLine(System.Array.Find(cars, car => car == "BMW"));

            //findall

            string[] hi = { "A", "B", "C", "B", "D" };
            string[] carss = System.Array.FindAll(hi, car => car=="B");

            foreach (var item in carss)
            {
                Console.WriteLine(item);

            }
            //contains
            string[] check = System.Array.FindAll(cars, car => car.Contains("B"));

            foreach (var item in check)
            {
                Console.WriteLine(item);
                
            }



           
        }
    }
}
