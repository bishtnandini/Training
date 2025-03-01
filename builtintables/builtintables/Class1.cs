using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace builtintables
{
    public class Class1 : IPlugin

    {

        public void Execute(IServiceProvider serviceProvider) 
        {
            
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
           
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);

            if (context.PrimaryEntityName == "account") 
            {
                Entity accountRecord = service.Retrieve("account", context.PrimaryEntityId, new ColumnSet("name", "telephone1"));
                string accountName = accountRecord.GetAttributeValue<string>("name");
                string phoneNumber = accountRecord.GetAttributeValue<string>("telephone1");

                Entity contactRecord = new Entity("contact");
                contactRecord["fullname"] = accountName; 
                contactRecord["telephone1"] = phoneNumber; 

              
                contactRecord["parentcustomerid"] = new EntityReference("account", context.PrimaryEntityId);
                contactRecord["accountrolecode"] = new OptionSetValue(2);
                contactRecord["creditlimit"] = new Money(100);
               
                contactRecord["donotphone"] = true;
                contactRecord["numberofchildren"] = 0;

                Guid contactId = service.Create(contactRecord);




            }
        }
    }
}
