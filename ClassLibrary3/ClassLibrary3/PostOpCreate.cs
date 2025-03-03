using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace ClassLibrary3
{
    public class PostOpCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));


            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);


            if (context.PrimaryEntityName == "cr371_acc")
            {

                Entity accountRecord = service.Retrieve("cr371_acc", context.PrimaryEntityId, new ColumnSet("cr371_name", "cr371_mobileno"));

                if (accountRecord != null)
                {
                    string accountName = accountRecord.GetAttributeValue<string>("cr371_name");
                    string phoneNumber = accountRecord.GetAttributeValue<string>("cr371_mobileno");


                    Entity contactRecord = new Entity("cr371_cuss");


                    contactRecord["cr371_fullname"] = accountName;
                    contactRecord["cr371_mobileno"] = phoneNumber;
                    contactRecord["cr371_parentcustomerid"] = new EntityReference("cr371_acc", context.PrimaryEntityId); // Lookup to Account
                    contactRecord["cr371_accountrolecode"] = new OptionSetValue(2);
                    contactRecord["cr371_creditlimit"] = new Money(100);
                    contactRecord["cr371_donotphone"] = true;
                    contactRecord["cr371_nmberofchildren"] = 0;


                    Guid contactId = service.Create(contactRecord);
                }
            }
        }
    }
}
