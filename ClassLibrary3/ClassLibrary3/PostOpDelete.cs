using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace ClassLibrary3
{
    public class PostOpDelete : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);

            if (context.PrimaryEntityName == "cr371_acc" && context.MessageName.ToLower() == "delete")
            {
                DeleteContacts(service, context.PrimaryEntityId);
            }
        }

        private void DeleteContacts(IOrganizationService service, Guid accountId)
        {
            // Query to fetch contacts related to the deleted account
            QueryExpression query = new QueryExpression("cr371_cuss")
            {
                ColumnSet = new ColumnSet("cr371_cussid"),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression("cr371_parentcustomerid", ConditionOperator.Equal, accountId)
                    }
                }
            };

            EntityCollection contacts = service.RetrieveMultiple(query);

            foreach (Entity contact in contacts.Entities)
            {
                service.Delete("cr371_cuss", contact.Id);
            }
        }
    }
}
