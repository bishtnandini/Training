using System;


namespace PExceptionHandling
{
    class InvalidAgeException : Exception
    {
        public InvalidAgeException(string message) : base(message) { }
    }

    class Program
    {
        static void CheckAge(int age)
        {
            if (age < 18)
            {
                throw new InvalidAgeException("Age should be greater than 18.");
            }
        }

        static void Number(int num)
        {
            if (num < 0)
            {
                throw new ArgumentException("Number can't be less than 0.");
            }
        }

        static void Main(string[] args)
        {
            try
            {
                int num = 10;
                int num2 = 0;
                // Console.WriteLine(num / num2); 

                int[] arr = new int[5];
                // Console.WriteLine(arr[6]); 

                // Number(-5); 
                CheckAge(15);
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Caught: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Execution completed. Cleaning up resources if needed.");
            }
        }
    }
}
