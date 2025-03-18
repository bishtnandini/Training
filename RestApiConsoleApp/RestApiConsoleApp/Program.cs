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
