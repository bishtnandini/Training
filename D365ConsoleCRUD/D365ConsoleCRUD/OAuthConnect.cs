using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;


namespace D365ConsoleCRUD
{
   public class OAuthConnect
    {
        
        public  CrmServiceClient ConnectWithOAuth()
        {
            Console.WriteLine("Connecting to D365 server....");
            string authType = "OAuth";
            string userName = "nandini@llcscg.com";
           
            string password = "Symbiotic1234";
            string url = "https://org346b45b2.crm.dynamics.com";
            string appId = "251bc6da-4b84-439b-9eb6-65745883a411";
            string reDirectURI ="https://localhost";
            string loginPrompt = "Auto";
            string ConnectionString = string.Format("AuthType ={0}; Username={1}; Password ={2}; Url= {3}; AppId={4}; RedirectUri={5}; LoginPrompt={6}",
                authType, userName, password, url, appId, reDirectURI, loginPrompt);

            CrmServiceClient svc = new CrmServiceClient(ConnectionString);
            return svc;
        }

        public void PerformCRUD(CrmServiceClient svc)
        {
            //create
            var myContact = new Entity("contact");
            myContact.Attributes["lastname"] = "bisht";
            myContact.Attributes["firstname"] = "nandini";
            myContact.Attributes["jobtitle"] = "xyz";
            Guid RecordID = svc.Create(myContact);
            Console.WriteLine("contact create with ID- " + RecordID);

            
            //retrieve
            Entity contact = svc.Retrieve("contact", new Guid(""), new ColumnSet("firstname", "lastname"));
            Console.WriteLine("contact lastname is -" + contact.Attributes["lastname"]);

            //Retreive multiple record
            QueryExpression qe = new QueryExpression("contact");
            qe.ColumnSet = new ColumnSet("firstname", "lastname");
            EntityCollection ec = svc.RetrieveMultiple(qe);

            for(int i = 0; i < ec.Entities.Count; i++)
            {
                if (ec.Entities[i].Attributes.ContainsKey("firstname"))
                {
                    Console.WriteLine(ec.Entities[i].Attributes["firstname"]);
                }
            }
            Console.WriteLine("retrieved all contacts....");

            //update
            Entity entContact = new Entity("contact");
            entContact.Id = RecordID;
            entContact.Attributes["lastname"] = "hey";
            svc.Update(entContact);
            Console.WriteLine("contact last name updated ....");

            //delete
            svc.Delete("contact", RecordID);

            //Execute
            Entity acc = new Entity("account");
            //acc["name"] = "";
            var createRequest = new CreateRequest()
            {
                Target = acc
            };
            svc.Execute(createRequest);

            //Execute Multiple
            var request = new ExecuteMultipleRequest()
            {
                Requests = new OrganizationRequestCollection(),
                Settings = new ExecuteMultipleSettings
                {
                    ContinueOnError = false,
                    ReturnResponses = true
                }
            };

            Entity acc1 = new Entity("account");
            acc1["name"] = "Soft1";
            Entity acc2 = new Entity("account");
            acc1["name"] = "Soft2";

            var createRequest1 = new CreateRequest()
            {
                Target = acc1
            };
            var createRequest2 = new CreateRequest()
            {
                Target = acc2
            };

            request.Requests.Add(createRequest1);
            request.Requests.Add(createRequest2);
            var response = (ExecuteMultipleResponse)svc.Execute(request);

           


        }
    }
}
