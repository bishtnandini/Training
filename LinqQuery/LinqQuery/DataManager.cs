using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQPractical
{
    internal class Employee
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }
    }
   internal static class DataManager
    {
        internal static List<Employee> GetData()
        {
            return new List<Employee>
            {
                new Employee { ID = 1, FirstName = "Happy", LastName = "Smith", Age = 30, Department = "Sales", Salary = 200000 },
                new Employee { ID = 2, FirstName = "John", LastName = "Doe", Age = 28, Department = "Marketing", Salary = 150000 },
                new Employee { ID = 3, FirstName = "Jane", LastName = "Brown", Age = 35, Department = "HR", Salary = 180000 },
                new Employee { ID = 4, FirstName = "Robert", LastName = "Johnson", Age = 40, Department = "Finance", Salary = 220000 },
                new Employee { ID = 5, FirstName = "Emily", LastName = "Davis", Age = 27, Department = "IT", Salary = 1700000000 },
                new Employee { ID = 6, FirstName = "Michael", LastName = "Wilson", Age = 32, Department = "Operations", Salary = 1600000 },
                new Employee { ID = 7, FirstName = "Sophia", LastName = "Moore", Age = 29, Department = "Sales", Salary = 19000000 },
                new Employee { ID = 8, FirstName = "Daniel", LastName = "Taylor", Age = 38, Department = "Legal", Salary = 2100000 },
                new Employee { ID = 9, FirstName = "Olivia", LastName = "Anderson", Age = 26, Department = "Customer Service", Salary = 1400000 },
                new Employee { ID = 10, FirstName = "William", LastName = "Thomas", Age = 31, Department = "Engineering", Salary = 230000 },
                new Employee { ID = 11, FirstName = "Isabella", LastName = "White", Age = 34, Department = "Procurement", Salary = 175000},
                new Employee { ID = 12, FirstName = "Isabella", LastName = "AA", Age = 34, Department = "Procurement", Salary = 175000}
            };
        }
    }
}
