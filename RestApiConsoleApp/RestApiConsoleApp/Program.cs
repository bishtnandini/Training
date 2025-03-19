using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    private static readonly HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://jsonplaceholder.typicode.com/")
    };

    static async Task Main()
    {
        while (true)
        {
            Console.WriteLine("\nSelect an operation:");
            Console.WriteLine("1. GET (perticular todo)");
            Console.WriteLine("2. GET (Paginated)");
            Console.WriteLine("3. GET (From JSON)");
            Console.WriteLine("4. POST");
            Console.WriteLine("5. POST As JSON");
            Console.WriteLine("6. PUT");
            Console.WriteLine("7. PUT As JSON");
            Console.WriteLine("8. PATCH");
            Console.WriteLine("9. DELETE");
            Console.WriteLine("10. HEAD");
            Console.WriteLine("11. Exit");
            Console.Write("Enter your choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

            switch (choice)
            {
                case 1:
                    await GetAsync(sharedClient);
                    break;
                case 2:
                    await GetPaginatedAsync(sharedClient);
                    break;
                case 3:
                    await GetFromJsonAsync(sharedClient);
                    break;
                case 4:
                    await PostAsync(sharedClient);
                    break;
                case 5:
                    await PostAsJsonAsync(sharedClient);
                    break;
                case 6:
                    await PutAsync(sharedClient);
                    break;
                case 7:
                    await PutAsJsonAsync(sharedClient);
                    break;
                case 8:
                    await PatchAsync(sharedClient);
                    break;
                case 9:
                    await DeleteAsync(sharedClient);
                    break;
                case 10:
                    await HeadAsync(sharedClient);
                    break;
                case 11:
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid choice! Please try again.");
                    break;
            }
        }
    }

    static async Task GetAsync(HttpClient client)
    {
        Console.Write("Enter ID to fetch: ");
        string id = Console.ReadLine();
        try
        {
            using HttpResponseMessage response = await client.GetAsync($"todos/{id}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"GET failed: {ex.Message}");
        }
    }

    static async Task GetFromJsonAsync(HttpClient client)
    {
        Console.Write("Enter ID to fetch: ");
        string id = Console.ReadLine();
        try
        {
            var result = await client.GetFromJsonAsync<Todo>($"todos/{id}");
            Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"GET failed: {ex.Message}");
        }
    }

    static async Task GetPaginatedAsync(HttpClient client)
    {
        Console.Write("Enter the page number: ");
        int pageNumber = int.TryParse(Console.ReadLine(), out int page) ? page : 1;

        Console.Write("Enter number of items per page: ");
        int pageSize = int.TryParse(Console.ReadLine(), out int size) ? size : 5;

        int startIndex = (pageNumber - 1) * pageSize; //starting index of pagination

        try
        {
            using HttpResponseMessage response = await client.GetAsync($"todos?_start={startIndex}&_limit={pageSize}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            Console.WriteLine($"GET failed: Resource not found (404) - {ex.Message}");
        }
    }

    static async Task PostAsync(HttpClient client)
    {
        Console.Write("Enter userId: ");
        int userId = int.Parse(Console.ReadLine());

        Console.Write("Enter id: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Enter title: ");
        string title = Console.ReadLine();

        var data = new { userId, id, title, completed = false };

        try
        {
            using StringContent jsonContent = new(
               JsonSerializer.Serialize(data),
               Encoding.UTF8,
               "application/json");

            using HttpResponseMessage response = await client.PostAsync("todos", jsonContent);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            Console.WriteLine($"POST failed: Unauthorized (401) - {ex.Message}");
        }
    }



    static async Task PostAsJsonAsync(HttpClient client)
    {
        Console.Write("Enter userId: ");
        int userId = int.Parse(Console.ReadLine());

        Console.Write("Enter id: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Enter title: ");
        string title = Console.ReadLine();

        var data = new Todo { UserId = userId, Id = id, Title = title, Completed = false };

        try
        {
            using HttpResponseMessage response = await client.PostAsJsonAsync(
                "todos", data);

            response.EnsureSuccessStatusCode().WriteRequestToConsole();

            var todo = await response.Content.ReadFromJsonAsync<Todo>();
            Console.WriteLine($"{todo}\n");

           
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"POST failed: {ex.Message}");
        }
    }

    static async Task PutAsync(HttpClient client)
    {
        Console.Write("Enter userId: ");
        int userId = int.Parse(Console.ReadLine());

        Console.Write("Enter id: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Enter updated title: ");
        string title = Console.ReadLine();

        var data = new { userId, id, title, completed = true };

        try
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await client.PutAsync($"todos/{id}", jsonContent);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            Console.WriteLine($"PUT failed: Unauthorized (401) - {ex.Message}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"PUT failed: HTTP error {ex.StatusCode} - {ex.Message}");
        }
    }

    static async Task PutAsJsonAsync(HttpClient client)
    {
        Console.Write("Enter userId: ");
        int userId = int.Parse(Console.ReadLine());

        Console.Write("Enter id: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Enter updated title: ");
        string title = Console.ReadLine();

        var data = new Todo { UserId = userId, Id = id, Title = title, Completed = true };

        try
        {
            using HttpResponseMessage response = await client.PutAsJsonAsync($"todos/{id}", data);
            response.EnsureSuccessStatusCode();

            var todo = await response.Content.ReadFromJsonAsync<Todo>();
            Console.WriteLine($"{ todo}\n");
           // Console.WriteLine(JsonSerializer.Serialize(todo, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"PUT failed: {ex.Message}");
        }
    }

    static async Task PatchAsync(HttpClient client)
    {
        Console.Write("Enter resource ID to modify: ");
        string id = Console.ReadLine();
        try
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new { completed = true }),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await client.PatchAsync($"todos/{id}", jsonContent);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"PATCH failed: {ex.Message}");
        }
    }

    static async Task DeleteAsync(HttpClient client)
    {
        Console.Write("Enter resource ID to delete: ");
        string id = Console.ReadLine();
        try
        {
            var response = await client.DeleteAsync($"todos/{id}");
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Resource deleted successfully.");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"DELETE failed: {ex.Message}");
        }
    }

    static async Task HeadAsync(HttpClient httpClient)
    {
        try
        {
            using HttpRequestMessage request = new(HttpMethod.Head, "https://www.example.com");

            using HttpResponseMessage response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            foreach (var header in response.Headers)
            {
                Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"HEAD request failed: {ex.Message}");
        }
    }
}


public record class Todo
{
    public int? UserId { get; init; }
    public int? Id { get; init; }
    public string? Title { get; init; }
    public bool? Completed { get; init; }
}


static class HttpResponseMessageExtensions
{
    internal static void WriteRequestToConsole(this HttpResponseMessage response)
    {
        if (response is null) return;
        var request = response.RequestMessage;
        Console.Write($"{request?.Method} ");
        Console.Write($"{request?.RequestUri} ");
        Console.WriteLine($"HTTP/{request?.Version}");
    }
}


/*
 using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

class Program
{
    private static readonly HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://jsonplaceholder.typicode.com"),
    };

    static async Task Main()
    {
        Console.WriteLine("Starting HTTP Client Example...");

        try
        {
               await GetAsync(sharedClient);
               await GetPaginatedAsync(sharedClient);
               await GetFromJsonAsync(sharedClient);
               await PostAsync(sharedClient);
               await PostAsJsonAsync(sharedClient);
               await PutAsync(sharedClient);
               await PutAsJsonAsync(sharedClient);
               await PatchAsync(sharedClient);
               await DeleteAsync(sharedClient);
               await HeadAsync(sharedClient);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        Console.WriteLine("All tasks completed.");
    }

    static async Task GetAsync(HttpClient httpClient)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync("todos/3");
            response.EnsureSuccessStatusCode().WriteRequestToConsole();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            Console.WriteLine($"GET failed: Resource not found (404) - {ex.Message}");
        }
    }
    static async Task GetPaginatedAsync(HttpClient httpClient)
    {
        int pageNumber = 1;
        const int pageSize = 5;
        string userInput;

        do
        {
            int startIndex = (pageNumber - 1) * pageSize;
            string url = $"todos?_start={startIndex}&_limit={pageSize}";

            try
            {
                using HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode().WriteRequestToConsole();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"{jsonResponse}\n");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine($"GET failed: Resource not found (404) - {ex.Message}");
                break;
            }

            Console.Write("Do you want to fetch more? (yes/no): ");
            userInput = Console.ReadLine()?.Trim().ToLower();

            if (userInput == "yes")
            {
                pageNumber++;
            }

        } while (userInput == "yes");
    }


    static async Task GetFromJsonAsync(HttpClient httpClient)
    {
        try
        {
            var todos = await httpClient.GetFromJsonAsync<List<Todo>>("todos?userId=1&completed=false");
            Console.WriteLine("GET https://jsonplaceholder.typicode.com/todos?userId=1&completed=false HTTP/1.1");
            todos?.ForEach(Console.WriteLine);
            Console.WriteLine();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            Console.WriteLine($"GET failed: Resource not found (404) - {ex.Message}");
        }
    }

    static async Task PostAsync(HttpClient httpClient)
    {
        try
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new { userId = 77, id = 1, title = "write code sample", completed = false }),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await httpClient.PostAsync("todos", jsonContent);
            response.EnsureSuccessStatusCode().WriteRequestToConsole();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            Console.WriteLine($"POST failed: Unauthorized (401) - {ex.Message}");
        }
    }

    static async Task PostAsJsonAsync(HttpClient httpClient)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "todos", new Todo(UserId: 9, Id: 99, Title: "Show extensions", Completed: false));

            response.EnsureSuccessStatusCode().WriteRequestToConsole();

            var todo = await response.Content.ReadFromJsonAsync<Todo>();
            Console.WriteLine($"{todo}\n");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            Console.WriteLine($"POST failed: Unauthorized (401) - {ex.Message}");
        }
    }

    static async Task PutAsync(HttpClient httpClient)
    {
        try
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new { userId = 1, id = 1, title = "foo bar", completed = false }),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await httpClient.PutAsync("todos/1", jsonContent);
            response.EnsureSuccessStatusCode().WriteRequestToConsole();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            Console.WriteLine($"POST failed: Unauthorized (401) - {ex.Message}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"POST failed: HTTP error {ex.StatusCode} - {ex.Message}");
        }
    }

    static async Task PutAsJsonAsync(HttpClient httpClient)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PutAsJsonAsync(
                "todos/5",
                new Todo(Title: "partially update todo", Completed: true));

            response.EnsureSuccessStatusCode().WriteRequestToConsole();

            var todo = await response.Content.ReadFromJsonAsync<Todo>();
            Console.WriteLine($"{todo}\n");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            Console.WriteLine($"POST failed: Unauthorized (401) - {ex.Message}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"POST failed: HTTP error {ex.StatusCode} - {ex.Message}");
        }
    }

    static async Task PatchAsync(HttpClient httpClient)
    {
        try
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new { completed = true }),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await httpClient.PatchAsync("todos/1", jsonContent);
            response.EnsureSuccessStatusCode().WriteRequestToConsole();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            Console.WriteLine($"PATCH failed: Resource not found (404) - {ex.Message}");
        }
    }

    static async Task DeleteAsync(HttpClient httpClient)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.DeleteAsync("todos/1");
            response.EnsureSuccessStatusCode().WriteRequestToConsole();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            Console.WriteLine($"PATCH failed: Resource not found (404) - {ex.Message}");
        }
    }

    static async Task HeadAsync(HttpClient httpClient)
    {
        try
        {
            using HttpRequestMessage request = new(HttpMethod.Head, "https://www.example.com");

            using HttpResponseMessage response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode().WriteRequestToConsole();

            foreach (var header in response.Headers)
            {
                Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"HEAD request failed: {ex.Message}");
        }
    }
}

public record class Todo(int? UserId = null, int? Id = null, string? Title = null, bool? Completed = null);

static class HttpResponseMessageExtensions
{
    internal static void WriteRequestToConsole(this HttpResponseMessage response)
    {
        if (response is null) return;
        var request = response.RequestMessage;
        Console.Write($"{request?.Method} ");
        Console.Write($"{request?.RequestUri} ");
        Console.WriteLine($"HTTP/{request?.Version}");
    }
}

 */