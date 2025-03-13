using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;

namespace D365ConsoleCRUD
{
    class Program
    {
        static void Main(string[] args)
        {
            #region OAUTH
            OAuthConnect oAuth = new OAuthConnect();
            CrmServiceClient svc = oAuth.ConnectWithOAuth();

            if (svc.IsReady)
            {
                Console.WriteLine("connected to D365 server....");
                oAuth.PerformCRUD(svc);
            }
            else
            {
                Console.WriteLine("CrmServiceClient is NOT ready.");
                Console.WriteLine($"Error: {svc.LastCrmError}");
                Console.WriteLine($"Extended Error: {svc.LastCrmException}");
                Console.WriteLine("Couldn't connected to D365 server....");
            }
            #endregion
        }
    }
}
