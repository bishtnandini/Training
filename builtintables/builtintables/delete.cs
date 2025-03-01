using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace builtintables
{
    public class delete : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);

            if (context.PrimaryEntityName == "account" && context.MessageName.ToLower() == "delete")
            {
                DeleteContacts(service, context.PrimaryEntityId);
            }
        }

        private void DeleteContacts(IOrganizationService service, Guid accountId)
        {
            // Query to fetch contacts related to the deleted account
            QueryExpression query = new QueryExpression("contact")
            {
                ColumnSet = new ColumnSet("contactid"),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression("parentcustomerid", ConditionOperator.Equal, accountId)
                    }
                }
            };

            EntityCollection contacts = service.RetrieveMultiple(query);

            foreach (Entity contact in contacts.Entities)
            {
                service.Delete("contact", contact.Id);
            }
        }
    
    }
}
