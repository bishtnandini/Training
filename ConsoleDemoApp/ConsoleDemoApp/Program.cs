// See https://aka.ms/new-console-template for more information
/*
Console.WriteLine("Hello, World!");
Console.Write("Hello1");
Console.WriteLine("Hello2");

Console.WriteLine("pls enter your name");
string? name = Console.ReadLine();

Console.WriteLine($"Hi {name}");

*/

using System;

public class cardHolder
{
    string cardNum;
    int pin;
    string firstName;
    string lastName;
    double balance;

    public cardHolder(string cardNum, int pin, string firstName, string lastName, double balance)
    {
        this.cardNum = cardNum;
        this.pin = pin;
        this.firstName = firstName;
        this.lastName = lastName;
        this.balance = balance;
    }

    //geter
    public string getNum()
    {
        return cardNum;
    }
    public int getPin()
    {
        return pin;
    }
    public string getFirstName()
    {
        return firstName;
    }
    public string getLastName()
    {
        return lastName;
    }
    public double getBalance()
    {
        return balance;
    }

    //seter
    public void setNum(string newCardNum)
    {
        cardNum = newCardNum;
    }
    public void setPin(int newPin)
    {
        pin = newPin;
    }
    public void setFirstName(string newFirstName)
    {
        firstName = newFirstName;
    }
    public void setLastName(string newLastName)
    {
        lastName = newLastName;
    }
    public void setBalance(double newBalance)
    {
        balance = newBalance;
    }

    public static void Main(string[] args)
    {
        void printOptions()
        {
            Console.WriteLine("Pls choose from one of the following options");
            Console.WriteLine("1. Deposit");
            Console.WriteLine("2. Withdraw");
            Console.WriteLine("3. Show Balance");
            Console.WriteLine("4. Exit");
        }

        void deposit(cardHolder currentUser)//carholder obj to the current user(Passing whole obj to the current user)
        {
            Console.WriteLine("How much ₹₹ would u like to deposit? ");
            double deposit = Double.Parse(Console.ReadLine());
            currentUser.setBalance(deposit + currentUser.getBalance());
            Console.WriteLine($"Thank you your current balance is {currentUser.getBalance()}");


        }

        void withdraw(cardHolder currentUser)//carholder obj to the current user(Passing whole obj to the current user)
        {
            Console.WriteLine("How much ₹₹ would u like to deposit? ");
            double withdrawal = Double.Parse(Console.ReadLine());
            
            if(currentUser.getBalance()> withdrawal)
            {
                Console.WriteLine("Insufficient balance ");
            }
            else
            {
                double newBalance = currentUser.getBalance() - withdrawal;
                currentUser.setBalance(newBalance);
                Console.WriteLine("Thank you , you're good to go!");
            }
        }

        void balance(cardHolder currentUser)
        {
            Console.WriteLine($"Hi {currentUser.firstName} your Current balance {currentUser.getBalance()}");
        }

        List<cardHolder> cardHolders = new List<cardHolder>();
        cardHolders.Add(new cardHolder("44444444", 1234, "John", "Griffith", 122.2));
        cardHolders.Add(new cardHolder("33333333", 1234, "Ashley", "Jones", 150.2));
        cardHolders.Add(new cardHolder("22222222", 1234, "Dawn", "Smith", 180.2));
        cardHolders.Add(new cardHolder("11111111", 1234, "Frida", "Wason", 190.2));

        //promt user
        Console.WriteLine("Hi Welcome to XYZ ATM");
        Console.WriteLine("Pls insert your debit card ");
        string debCardNum = "";

        cardHolder currentUser;

        while (true)
        {
            try
            {
                debCardNum = Console.ReadLine();
                //check against the list
                currentUser = cardHolders.FirstOrDefault(a => a.cardNum == debCardNum);
                if(currentUser != null) { break; }
                else {
                    Console.WriteLine("Sorry but the card not recognized,Please try again");
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("Sorry but the card not recognized,Please try again");
            }
        }
        Console.WriteLine("pls enetr your pin : ");
        int userPin = 0;
        while (true)
        {
            try
            {
                userPin = int.Parse(Console.ReadLine());
                
               
                if (currentUser.getPin() == userPin) { break; }
                else
                {
                    Console.WriteLine("Incorrect pin. Pls try again");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Incorrect pin.Pls try again");
            }
        }

        Console.WriteLine($"Welcome {currentUser.getFirstName()}");
        int option = 0;

        do
        {
            printOptions();
            try
            {
                option = int.Parse(Console.ReadLine());

            }
            catch
            {

            }
            if(option == 1)
            {
                deposit(currentUser);
            }
            else if(option == 2)
            {
                withdraw(currentUser);
            }
            else if (option == 3)
            {
                balance(currentUser);
            }
            else if (option == 4)
            {
                break;
            }
            else
            {
                option = 0;

            }
        }
        while (option != 4);
        Console.WriteLine("Thank you have a nice day");



    }
}

