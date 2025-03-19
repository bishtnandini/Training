using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQPractical
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("LINQ Practical");
            List<Employee> employees = DataManager.GetData();

          
            //select => projection obj type change
            var names = employees.Select(e => e.FirstName);
            foreach (var item in names)
            {
                Console.WriteLine(item);
            }
            //OR
            Console.WriteLine("just checking the diff");
            IEnumerable<string> naam = employees.Select(e => e.FirstName).Distinct();
            foreach (string it in naam)
            {
                Console.WriteLine(it);
            }

            //order the collection
            //employees = employees.OrderBy(e => e.FirstName).ThenBy(e=>e.LastName).ToList();

            //orderbydesc
            employees = employees.OrderByDescending(e => e.FirstName).ThenByDescending(e => e.LastName).ToList();

            /*fiter (always return collection so when u want a collection from collection u can choose this
            but if u want to fetch a single value use other query like first )*/
            //Console.WriteLine("filter");
            var empl= employees.Where(e => e.Age >= 30 && e.Salary >20).ToList();
            empl.ForEach(e => Console.WriteLine(e.FirstName));


            //first return first matching value
            Console.WriteLine("Firsttttttttttttttttt");
            //it will throw an error if value doestn't exists 
            var emp2 = employees.First(e=>e.FirstName== "Isabella");
            Console.WriteLine(emp2.ID);
            //if value doesnot exists it will give defalut value in class default value is null in coll it have some value
            var emp3 = employees.FirstOrDefault(e => e.FirstName == "Isabella1");
            if(emp3!= null)
            {
                Console.WriteLine(emp3.FirstName);

            }
            else
            {
                Console.WriteLine("not found ");
            }

            //last return last matching value
            Console.WriteLine("lastttttttttttttt");
            //it will throw an error if value doestn't exists 
            var emp4 = employees.Last(e => e.FirstName == "Isabella");
            Console.WriteLine(emp4.ID);
            //if value doesnot exists it will give defalut value in class default value is null in coll it have some value
            var emp5 = employees.LastOrDefault(e => e.FirstName == "Isabella1");
            if (emp5 != null)
            {
                Console.WriteLine(emp3.FirstName);

            }
            else
            {
                Console.WriteLine("not found ");
            }

            //single collection must have unique value otherwise it will throw error('Sequence contains more than one matching element')
            // var emp6 = employees.Single(e => e.FirstName == "Isabella");

            // Take (first n) 
            Console.WriteLine("takeeeeeeee");
            var emp6 = employees.Take(2);
            foreach (var item in emp6)
            {
                Console.WriteLine(item.FirstName);
            }

            //skip few ele like and print rest of the ele
            Console.WriteLine("skippppppppppppp");
            var emp7 = employees.Skip(2);
            foreach (var item in emp7)
            {
                Console.WriteLine(item.FirstName);
            }

            //combine skip 2 then take 5
            Console.WriteLine("combineeeeeeeeeeeeee");
            var emp8 = employees.Skip(2).Take(5);
            foreach (var item in emp8)
            {
                Console.WriteLine(item.ID);
            }

            //entire collection filter by some condition
            // employees = employees.DistinctBy(e => e.FirstName).ToList();

            //aggregat
            //if any value in collection age > 30 it will return true
            Console.WriteLine(employees.Any(e=>e.Age>30));
            //if all the value in collection age > 30 it will return true otherwaise false
            Console.WriteLine(employees.All(e => e.Age > 30));
            //count no of emp whose age is > 30
            Console.WriteLine(employees.Count(e => e.Age > 30));
            //Sum
            Console.WriteLine(employees.Sum(e => e.Salary));
            //avg
            Console.WriteLine(employees.Average(e => e.Salary));
            //max
            Console.WriteLine(employees.Max(e => e.Salary));
            //min
            Console.WriteLine(employees.Min(e => e.Salary));
            //minby it will retuen the emp not salary
         // Employee employee = employees.MinBy(e => e.Salary);


            //foreach
            
            employees.ForEach(emp => Console.WriteLine(
            "ID : {0} , FirstName :{1}, LastName :{2} Age :{3}, Department :{4}, Salary:{5}",
             emp.ID, emp.FirstName, emp.LastName, emp.Age, emp.Department, emp.Salary));
            


        }
    }
}
