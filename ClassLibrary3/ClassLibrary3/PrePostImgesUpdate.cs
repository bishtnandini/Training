using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace ClassLibrary3
{
    public class PrePostImgesUpdate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);

            tracingService.Trace("Plugin Execution Started.");

            if (context.PrimaryEntityName == "cr371_acc")
            {
              
                if (context.Depth > 1) return; // Prevent infinite loops
                tracingService.Trace("Processing entity: cr371_acc");

                // Retrieving Pre-Image and Post-Image
                Entity preImage = context.PreEntityImages.Contains("PreImage") ? context.PreEntityImages["PreImage"] : null;
                Entity postImage = context.PostEntityImages.Contains("PostImage") ? context.PostEntityImages["PostImage"] : null;

                // For Create: Use Default Values
                Money preMoneyValue = preImage != null && preImage.Contains("cr371_money") ? preImage.GetAttributeValue<Money>("cr371_money") : new Money(0);
                int preAddress = preImage != null && preImage.Contains("cr371_address1_freighttermscode") ? preImage.GetAttributeValue<OptionSetValue>("cr371_address1_freighttermscode").Value : 1;
                int preAddressCode = preImage != null && preImage.Contains("cr371_address1_addresstypecode") ? preImage.GetAttributeValue<OptionSetValue>("cr371_address1_addresstypecode").Value : 1;

                tracingService.Trace($"Pre-Image values - Money: {preMoneyValue.Value}, Address: {preAddress}, AddressCode: {preAddressCode}");

                // Updating Account Money Field (Only for Create)
                if (context.MessageName.ToLower() == "create")
                {
                    Entity updateAcc = new Entity("cr371_acc");
                    updateAcc.Id = context.PrimaryEntityId;
                    updateAcc["cr371_money"] = (preAddress == 1 && preAddressCode == 1) ? new Money(500) : new Money(600);
                    service.Update(updateAcc);
                    tracingService.Trace("Account money field updated.");
                }

                // Ensure Post-Image is available for updates
                if (context.MessageName.ToLower() == "update")
                {
                    if (postImage == null || !postImage.Contains("cr371_money"))
                    {
                        tracingService.Trace("Post-Image is missing or does not contain cr371_money. Exiting plugin.");
                        return;
                    }
                }

                Money updatedMoneyValue = postImage != null && postImage.Contains("cr371_money") ? postImage.GetAttributeValue<Money>("cr371_money") : new Money(0);
                tracingService.Trace($"Updated Money Value: {updatedMoneyValue.Value}");

                /*
               
                QueryExpression query = new QueryExpression("cr371_cuss")
                {
                    ColumnSet = new ColumnSet("cr371_fullname"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression("cr371_parentcustomerid", ConditionOperator.Equal, context.PrimaryEntityId)
                        }
                    }
                };
                */

                QueryExpression query = new QueryExpression("cr371_cuss")
                {
                    ColumnSet = new ColumnSet("cr371_creditlimit", "cr371_parentcustomerid"),
                    Criteria = new FilterExpression
                    {
                        FilterOperator = LogicalOperator.Or,  // <-- Allow broader matching
                        Conditions =
                        {
                            new ConditionExpression("cr371_parentcustomerid", ConditionOperator.Equal, context.PrimaryEntityId)
                        }
                    }
                };

                EntityCollection relatedCustomers = service.RetrieveMultiple(query);

                tracingService.Trace($"Found {relatedCustomers.Entities.Count} related customers.");

                // If no related customers, log the issue

                /*
                if (relatedCustomers.Entities.Count == 0)
                {
                    tracingService.Trace("No related customers found. Exiting plugin.");

                    return;
                }
                */
                if (relatedCustomers.Entities.Count == 0)
                {
                    throw new InvalidPluginExecutionException("No related customer records found.");
                }

                // Update cr371_creditlimit for each related cr371_cuss record
                foreach (Entity customer in relatedCustomers.Entities)
                {
                    tracingService.Trace($"Updating credit limit for customer ID: {customer.Id}");
                    Entity updateCustomer = new Entity("cr371_cuss")
                    {
                        Id = customer.Id
                    };
                    updateCustomer["cr371_creditlimit"] = new Money(updatedMoneyValue.Value * 1.2M); // Example: Setting credit limit as 1.2x of money
                    service.Update(updateCustomer);
                }

                tracingService.Trace("Credit limits updated successfully.");
            }
        }
    }
}
