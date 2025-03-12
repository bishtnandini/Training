using System;
using System.Collections.Generic;
using System.Linq;

public class Book
{
    string title;
    string author;
    int publicationYear;
    double price;
    int quantity;

    public Book(string title, string author, int publicationYear, double price, int quantity)
    {
        this.title = title;
        this.author = author;
        this.publicationYear = publicationYear;
        this.price = price;
        this.quantity = quantity;
    }

    public string getTitle() { 
        return title;
    }
    public string getAuthor() { 
        return author;
    }
    public int getPublicationYear() { 
        return publicationYear;
    }
    public double getPrice() {
        return price;
    }
    public int getQuantity() {
        return quantity;
    }
    public void setQuantity(int newQuantity) {
        quantity = newQuantity; 
    }

    public bool OrderBook(int orderQuantity)
    {
        if (orderQuantity > quantity)
        {
            Console.WriteLine($"Not enough stock available! Only {quantity} left.");
            return false;
        }
        quantity -= orderQuantity;
        Console.WriteLine($"Order placed successfully! {orderQuantity} copies of \"{title}\" ordered.");
        return true;
    }

    public virtual void ShowDetails()
    {
        Console.WriteLine($"Title: {title}, Author: {author}, Year: {publicationYear}, Price: {price} ₹, Quantity: {quantity}");
    }
}

public class EBook : Book
{
    double fileSize;

    public EBook(string title, string author, int publicationYear, double price, int quantity, double fileSize)
        : base(title, author, publicationYear, price, quantity)
    {
        this.fileSize = fileSize;
    }

    public override void ShowDetails()
    {
        Console.WriteLine($"[E-Book] Title: {getTitle()}, Author: {getAuthor()}, Year: {getPublicationYear()}, Price: {getPrice()} ₹, File Size: {fileSize} MB, Quantity: {getQuantity()}");
    }
}

public class PrintedBook : Book
{
    int pageCount;

    public PrintedBook(string title, string author, int publicationYear, double price, int quantity, int pageCount)
        : base(title, author, publicationYear, price, quantity)
    {
        this.pageCount = pageCount;
    }

    public override void ShowDetails()
    {
        Console.WriteLine($"[Printed Book] Title: {getTitle()}, Author: {getAuthor()}, Year: {getPublicationYear()}, Price: {getPrice()} ₹, Pages: {pageCount}, Quantity: {getQuantity()}");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        List<Book> library = new List<Book>
        {
            new EBook("C# Programming", "John Doe", 2022, 500, 5, 15.2),
            new PrintedBook("Data Structures", "Jane Smith", 2018, 700, 3, 450),
            new EBook("AI and Machine Learning", "Alice Johnson", 2021, 600, 2, 20.5),
            new PrintedBook("Database Management", "Robert Brown", 2017, 900, 4, 600)
        };

        Dictionary<string, int> orderedBooks = new Dictionary<string, int>();

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n=== XYZ Library Menu ===");
            Console.WriteLine("1. View All Books");
            Console.WriteLine("2. Order a Book");
            Console.WriteLine("3. View Ordered Books");
            Console.WriteLine("4. Search for a Book");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice (1-5): ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nAvailable Books:");
                    foreach (Book book in library)
                    {
                        book.ShowDetails();
                    }
                    break;

                case "2":
                    Console.Write("\nEnter the book title you want to order: ");
                    string orderTitle = Console.ReadLine();

                    Book orderBook = library.FirstOrDefault(b => b.getTitle().Equals(orderTitle, StringComparison.OrdinalIgnoreCase));

                    if (orderBook != null)
                    {
                        Console.Write("Enter quantity to order: ");
                        if (int.TryParse(Console.ReadLine(), out int orderQuantity) && orderQuantity > 0)
                        {
                            if (orderBook.OrderBook(orderQuantity))
                            {
                                if (orderedBooks.ContainsKey(orderTitle))
                                    orderedBooks[orderTitle] += orderQuantity;
                                else
                                    orderedBooks[orderTitle] = orderQuantity;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid quantity entered!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nSorry, this book is not available in the library.");
                    }
                    break;

                case "3":
                    Console.WriteLine("\nOrdered Books:");
                    if (orderedBooks.Count == 0)
                    {
                        Console.WriteLine("No books have been ordered yet.");
                    }
                    else
                    {
                        foreach (var order in orderedBooks)
                        {
                            Console.WriteLine($"Title: {order.Key}, Quantity Ordered: {order.Value}");
                        }
                    }
                    break;

                case "4":
                    Console.Write("\nEnter the book title you want to search: ");
                    string searchTitle = Console.ReadLine();

                    Book foundBook = library.FirstOrDefault(b => b.getTitle().Equals(searchTitle, StringComparison.OrdinalIgnoreCase));

                    if (foundBook != null)
                    {
                        Console.WriteLine("\nBook found!");
                        foundBook.ShowDetails();
                    }
                    else
                    {
                        Console.WriteLine("\nSorry, this book is not available in the library.");
                    }
                    break;

                case "5":
                    Console.WriteLine("\nThank you for using XYZ Library! Goodbye!");
                    exit = true;
                    break;

                default:
                    Console.WriteLine("\nInvalid choice! Please enter a number between 1 and 5.");
                    break;
            }
        }
    }
}
