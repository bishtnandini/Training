using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace ClassLibrary3
{
    public class PostOpUpdate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);

            if (context.PrimaryEntityName == "cr371_acc")
            {
                if (context.Depth > 1)
                    return;
                Entity accRecord = service.Retrieve("cr371_acc", context.PrimaryEntityId, new ColumnSet("cr371_address1_freighttermscode", "cr371_address1_addresstypecode"));
                int adress = accRecord.Contains("cr371_address1_freighttermscode") ? accRecord.GetAttributeValue<OptionSetValue>("cr371_address1_freighttermscode").Value : 1;
                int adressCode = accRecord.Contains("cr371_address1_addresstypecode") ? accRecord.GetAttributeValue<OptionSetValue>("cr371_address1_addresstypecode").Value : 1;

                Entity updatetoacc = new Entity("cr371_acc");
                updatetoacc.Id = context.PrimaryEntityId;

                if (adress == 1 && adressCode == 1)
                {
                    updatetoacc["cr371_money"] = new Money(500);
                }
                else
                {
                    updatetoacc["cr371_money"] = new Money(600);

                }

                service.Update(updatetoacc);

            }
        }
    }
}
