//directly using id and pass 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

namespace MSALDataverseCrud
{
    class Program
    {
        
        private static string dataverseUrl = "";  // Dataverse URL
        private static string username = ""; 
        private static string password = "";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Authenticating with Username & Password...");

            string connectionString = $"AuthType=OAuth; Url={dataverseUrl};" +
                                      $"Username={username}; Password={password};" +
                                      $"AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;" +  // Microsoft default App ID
                                      $"RedirectUri=http://localhost; LoginPrompt=Auto;";

            using (var serviceClient = new ServiceClient(connectionString))
            {
                if (serviceClient.IsReady)
                {
                    Console.WriteLine("Successfully connected to Dataverse!");
                    Console.WriteLine(" Retrieving records from Dataverse...");

                    while (true)
                    {
                        Console.WriteLine("\n Choose an operation:");
                        Console.WriteLine("1 Create Record");
                        Console.WriteLine("2 Retrieve Records");
                        Console.WriteLine("3 Update Record");
                        Console.WriteLine("4 Delete Record");
                        Console.WriteLine("5 Exit");
                        Console.Write(" Enter your choice: ");
                        string choice = Console.ReadLine();

                        switch (choice)
                        {
                            case "1":
                                CreateRecord(serviceClient);
                                break;
                            case "2":
                                RetrieveRecords(serviceClient);
                                break;
                            case "3":
                                UpdateRecord(serviceClient);
                                break;
                            case "4":
                                DeleteRecord(serviceClient);
                                break;
                            case "5":
                                Console.WriteLine(" Exiting...");
                                return;
                            default:
                                Console.WriteLine(" Invalid choice! Please enter a valid option.");
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine(" Failed to connect to Dataverse.");
                }
            }
        }

        static void CreateRecord(ServiceClient serviceClient)
        {
            Entity account = new Entity("account");
            Console.Write("➡ Enter Account Name: ");
            account["name"] = Console.ReadLine();

            Console.Write("➡ Enter mobile no : ");
            account["telephone1"] = Console.ReadLine();

           serviceClient.Create(account);
            Console.WriteLine($" Record Created Successfully!");
        }

        static void RetrieveRecords(ServiceClient serviceClient)
        {
            string tableName = "account";  

            QueryExpression query = new QueryExpression(tableName)
            {
                ColumnSet = new ColumnSet("name", "telephone1")  
            };

            EntityCollection results = serviceClient.RetrieveMultiple(query);

            Console.WriteLine($" Total Records Found: {results.Entities.Count}");
            foreach (Entity entity in results.Entities)
            {
                Console.WriteLine($" Name: {entity.GetAttributeValue<string>("name")}, " +
                                  $"Mobile No: {entity.GetAttributeValue<string>("telephone1")}");
            }
        }

        static void UpdateRecord(ServiceClient serviceClient)
        {
            Console.Write("➡ Enter Account Name to Update: ");
            string accountName = Console.ReadLine();

            // 🔹 Query to find the Account ID by Name
            QueryExpression query = new QueryExpression("account")
            {
                ColumnSet = new ColumnSet("accountid"),
                Criteria = new FilterExpression
                {
                    Conditions =
            {
                new ConditionExpression("name", ConditionOperator.Equal, accountName)
            }
                }
            };

            EntityCollection results = serviceClient.RetrieveMultiple(query);

            if (results.Entities.Count == 0)
            {
                Console.WriteLine($" No account found with the name: {accountName}");
                return;
            }

            List<Entity> accountList = results.Entities.ToList(); 

           
            if (accountList.Count > 1)
            {
                Console.WriteLine($" Multiple accounts found with the name '{accountName}'. Please select an ID:");

                foreach (Entity entity in accountList)
                {
                    Console.WriteLine($"➡ ID: {entity.Id}");
                }

                Console.Write("➡ Enter the Account ID to update: ");
                Guid selectedAccountId;
                if (!Guid.TryParse(Console.ReadLine(), out selectedAccountId))
                {
                    Console.WriteLine(" Invalid ID format.");
                    return;
                }

                accountList = accountList.Where(e => e.Id == selectedAccountId).ToList();
                if (accountList.Count == 0)
                {
                    Console.WriteLine(" The selected ID does not match any records.");
                    return;
                }
            }

            Entity account = accountList.First(); //  get the first entity
            Console.WriteLine($" Found Account: {accountName} (ID: {account.Id})");

            
            Console.Write("➡ Enter New Account Name: ");
            account["name"] = Console.ReadLine();

            Console.Write("➡ mobilr no: ");
            account["telephone1"] = Console.ReadLine();

            serviceClient.Update(account);
            Console.WriteLine(" Record Updated Successfully!");
        }


        static void DeleteRecord(ServiceClient serviceClient)
        {
            Console.Write("➡ Enter Account Name to Delete: ");
            string accountName = Console.ReadLine();

           
            QueryExpression query = new QueryExpression("account")
            {
                ColumnSet = new ColumnSet("accountid"),
                Criteria = new FilterExpression
                {
                    Conditions =
            {
                new ConditionExpression("name", ConditionOperator.Equal, accountName)
            }
                }
            };

            EntityCollection results = serviceClient.RetrieveMultiple(query);

            if (results.Entities.Count == 0)
            {
                Console.WriteLine($" No account found with the name: {accountName}");
                return;
            }

            foreach (Entity entity in results.Entities)
            {
                Guid accountId = entity.Id;
                serviceClient.Delete("account", accountId);
                Console.WriteLine($" Account '{accountName}' (ID: {accountId}) Deleted Successfully!");
            }
        }
    }
}



//using token

/*
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;

class Program
{
    private static readonly string clientId = "";  
    private static readonly string clientSecret = "";  
    private static readonly string tenantId = "";  
    private static readonly string dataverseUrl = "";
    private static readonly string authority = $"https://login.microsoftonline.com/{tenantId}";

    private static string accessToken;

    static async Task Main(string[] args)
    {
        // Step 1: Authenticate and Get Token
        accessToken = await Authenticate();

        if (!string.IsNullOrEmpty(accessToken))
        {
            Console.WriteLine("Authentication Successful!");

            // Step 2: Fetch Account Records
            await RetrieveRecords();
        }
    }

    
    private static async Task<string> Authenticate()
    {
        var app = ConfidentialClientApplicationBuilder.Create(clientId)
            .WithClientSecret(clientSecret)
            .WithAuthority(authority)
            .Build();

        string[] scopes = new string[] { $"{dataverseUrl}/.default" };

        try
        {
            var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            Console.WriteLine($" Access Token: {result.AccessToken}");
            return result.AccessToken;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Authentication Failed: {ex.Message}");
            return null;
        }
    }

    
    private static async Task RetrieveRecords()
    {
        string query = "/api/data/v9.0/accounts?$select=name,accountnumber";  // Query Accounts
        string requestUrl = $"{dataverseUrl}{query}";

        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(jsonResponse);
                JArray accounts = (JArray)result["value"];

                Console.WriteLine($"Total Records Found: {accounts.Count}");
                foreach (var account in accounts)
                {
                    Console.WriteLine($" Name: {account["name"]}, Account Number: {account["accountnumber"]}");
                }
            }
            else
            {
                Console.WriteLine($" Failed to retrieve records: {response.ReasonPhrase}");
            }
        }
    }
}

*/

//without using token and tenantId

/*
using System;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

class Program
{
    private static readonly string clientId = ""; 
    private static readonly string clientSecret = "";  
    //private static readonly string tenantId = ""; 
    private static readonly string dataverseUrl = "";

    static void Main(string[] args)
    {
        string connectionString = $@"
            AuthType=ClientSecret;
            Url={dataverseUrl};
            ClientId={clientId};
            ClientSecret={clientSecret};
            RequireNewInstance=True;";

        using (ServiceClient serviceClient = new ServiceClient(connectionString))
        {
            if (serviceClient.IsReady)
            {
                Console.WriteLine("🔹 Connection Successful!");

                // Fetch Account Records
                RetrieveRecords(serviceClient);
            }
            else
            {
                Console.WriteLine($" Connection Failed: {serviceClient.LastError}");
            }
        }
    }

    static void RetrieveRecords(ServiceClient serviceClient)
    {
        string tableName = "account";  // Dataverse Table Name

        QueryExpression query = new QueryExpression(tableName)
        {
            ColumnSet = new ColumnSet("name", "accountnumber")  // Fetch Account Name & Number
        };

        EntityCollection results = serviceClient.RetrieveMultiple(query);

        Console.WriteLine($" Total Records Found: {results.Entities.Count}");
        foreach (Entity entity in results.Entities)
        {
            Console.WriteLine($"➡ Name: {entity.GetAttributeValue<string>("name")}, " +
                              $"Account Number: {entity.GetAttributeValue<string>("accountnumber")}");
        }
    }
}

*/

