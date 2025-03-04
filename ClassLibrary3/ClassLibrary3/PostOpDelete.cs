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

            // Ensure this is a Delete event for 'cr371_acc' entity
            if (context.PrimaryEntityName == "cr371_acc" && context.MessageName.ToLower() == "delete")
            {
                Guid deletedAccountId = context.PrimaryEntityId;

                if (deletedAccountId == Guid.Empty)
                    return;

                IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);

                DeleteRelatedCussRecords(service, deletedAccountId);
            }
        }

        public void DeleteRelatedCussRecords(IOrganizationService service, Guid accountId)
        {
           
            QueryExpression query = new QueryExpression("cr371_cuss")
            {
                ColumnSet = new ColumnSet(false), 
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression("cr371_parentcustomerid", ConditionOperator.Equal, accountId)
                    }
                }
            };

            EntityCollection relatedRecords = service.RetrieveMultiple(query);

            // Ensure all related records are deleted
            foreach (Entity record in relatedRecords.Entities)
            {
                service.Delete("cr371_cuss", record.Id);
            }
        }
    }
}
