using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

class Program
{
    private static readonly HttpClient httpClient = new()
    {
        BaseAddress = new Uri("https://dummy.restapiexample.com/api/v1/")
    };

    static async Task Main()
    {
        while (true)
        {
            Console.WriteLine("\nSelect an operation:");
            Console.WriteLine("1. GET (particular employee)");
            Console.WriteLine("2. GET (Paginated)");
            Console.WriteLine("3. POST");
            Console.WriteLine("4. PUT");
            Console.WriteLine("5. DELETE");
            Console.WriteLine("6. Exit");

            Console.Write("Enter your choice: ");
            if (!int.TryParse(Console.ReadLine()?.Trim() ?? "0", out int choice)) continue;

            switch (choice)
            {
                case 1:
                    await GetAsync(httpClient);
                    break;
                case 2:
                    await GetPaginatedAsync(httpClient);
                    break;
                case 3:
                    await PostAsync(httpClient);
                    break;
                case 4:
                    await PutAsync(httpClient);
                    break;
                case 5:
                    await DeleteAsync(httpClient);
                    break;
                case 6:
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid choice! Please try again.");
                    break;
            }
        }
    }

    static async Task GetAsync(HttpClient httpClient)
    {
        Console.Write("Enter the ID of the employee you want to search: ");
        string id = Console.ReadLine()?.Trim() ?? "0";

        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"employee/{id}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var formattedJson = JsonNode.Parse(jsonResponse);
            Console.WriteLine("Response:");
            Console.WriteLine(formattedJson?.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"GET failed: {ex.Message}");
        }
    }

    static async Task GetPaginatedAsync(HttpClient httpClient)
    {
        Console.Write("Enter page number: ");
        int pageNumber = int.TryParse(Console.ReadLine()?.Trim() ?? "1", out int page) ? page : 1;

        Console.Write("Enter page size: ");
        int pageSize = int.TryParse(Console.ReadLine()?.Trim() ?? "5", out int size) ? size : 5;

        int startIndex = (pageNumber - 1) * pageSize;

        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"employees?_start={startIndex}&_limit={pageSize}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var formattedJson = JsonNode.Parse(jsonResponse);
            Console.WriteLine("Response:");
            Console.WriteLine(formattedJson?.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            Console.WriteLine($"GET failed: Resource not found (404) - {ex.Message}");
        }
    }

    static async Task PostAsync(HttpClient httpClient)
    {
        Console.Write("Enter employee ID: ");
        int id = int.TryParse(Console.ReadLine()?.Trim() ?? "55", out int userId) ? userId : 55;

        Console.Write("Enter employee name: ");
        string employeeName = Console.ReadLine()?.Trim() ?? "Unknown";

        Console.Write("Enter employee salary: ");
        int employeeSalary = int.TryParse(Console.ReadLine()?.Trim() ?? "155555", out int sal) ? sal : 155555;

        Console.Write("Enter employee age: ");
        int employeeAge = int.TryParse(Console.ReadLine()?.Trim() ?? "18", out int age) ? age : 18;

        string profileImage = ""; // Keeping it empty as per your original code.

        var data = new { id, employeeName, employeeSalary, employeeAge, profileImage };
        try
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await httpClient.PostAsync("create", jsonContent);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var formattedJson = JsonNode.Parse(jsonResponse);
            Console.WriteLine("Response:");
            Console.WriteLine(formattedJson?.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            Console.WriteLine($"POST failed: Unauthorized (401) - {ex.Message}");
        }
    }

    static async Task PutAsync(HttpClient client)
    {
        Console.Write("Enter employee ID: ");
        int id = int.TryParse(Console.ReadLine()?.Trim() ?? "55", out int userId) ? userId : 55;

        Console.Write("Enter employee name: ");
        string employeeName = Console.ReadLine()?.Trim() ?? "Unknown";

        Console.Write("Enter employee salary: ");
        int employeeSalary = int.TryParse(Console.ReadLine()?.Trim() ?? "155555", out int sal) ? sal : 155555;

        Console.Write("Enter employee age: ");
        int employeeAge = int.TryParse(Console.ReadLine()?.Trim() ?? "18", out int age) ? age : 18;

        string profileImage = ""; // Keeping it empty.

        var data = new { id, employeeName, employeeSalary, employeeAge, profileImage };

        try
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PutAsync($"update/{id}", jsonContent);
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

    static async Task DeleteAsync(HttpClient client)
    {
        Console.Write("Enter resource ID to delete: ");
        string id = Console.ReadLine()?.Trim() ?? "0";

        try
        {
            using HttpResponseMessage response = await client.DeleteAsync($"delete/{id}");
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Resource deleted successfully.");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"DELETE failed: {ex.Message}");
        }
    }
}
