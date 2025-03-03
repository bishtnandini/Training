using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;


namespace ClassLibrary3
{
    public class PreValiInput : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

           
            if (context.MessageName.ToLower() != "create" || context.PrimaryEntityName != "cr371_acc")
            {
                return;
            }

            // Get the Target Entity from Input Parameters
            if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity))
            {
                return;
            }

            Entity targetEntity = (Entity)context.InputParameters["Target"];

            // Validate Required Fields - Account Name
            if (!targetEntity.Contains("cr371_name") || string.IsNullOrWhiteSpace(targetEntity.GetAttributeValue<string>("cr371_name")))
            {
                throw new InvalidPluginExecutionException("Account Name (cr371_name) is required and cannot be empty.");
            }

            
            string accountName = targetEntity.GetAttributeValue<string>("cr371_name");

            // Prevent Duplicate Account Names
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            QueryExpression query = new QueryExpression("cr371_acc")
            {
                ColumnSet = new ColumnSet("cr371_name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("cr371_name", ConditionOperator.Equal, accountName)
                    }
                }
            };

            EntityCollection existingAccounts = service.RetrieveMultiple(query);
            if (existingAccounts.Entities.Count > 0)
            {
                throw new InvalidPluginExecutionException($"An account with the name '{accountName}' already exists. Please use a different name.");
            }
        }
    }
}
