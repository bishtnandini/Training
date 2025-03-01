using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace builtintables
{
   public class updatesample : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

         
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);
            if (context.PrimaryEntityName == "account")
            {
                if (context.Depth > 1)
                    return;
                Entity accountRecord = service.Retrieve("account", context.PrimaryEntityId, new ColumnSet("address1_freighttermscode", "address1_addresstypecode"));
                int addressTCode = accountRecord.Contains("address1_addresstypecode") ? accountRecord.GetAttributeValue<OptionSetValue>("address1_addresstypecode").Value : 1;
                int paymentTerms = accountRecord.Contains("address1_freighttermscode") ? accountRecord.GetAttributeValue<OptionSetValue>("address1_freighttermscode").Value : 1;

               
                Entity accountToUpdate = new Entity("account");
                accountToUpdate.Id = context.PrimaryEntityId;
                if (addressTCode == 1 && paymentTerms == 1)
                {
                    accountToUpdate["revenue"] = new Money(200);
                }

                else
                {
                    accountToUpdate["revenue"] = new Money(500);
                }

                service.Update(accountToUpdate);

            }
        }

    
    }
}
