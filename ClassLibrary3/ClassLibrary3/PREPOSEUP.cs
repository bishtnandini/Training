using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace ClassLibrary3
{
    public class PREPOSEUP : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            tracingService.Trace("Plugin Execution Started.");
            tracingService.Trace($"Primary Entity: {context.PrimaryEntityName}");
            tracingService.Trace($"Depth: {context.Depth}");

            if (context.PrimaryEntityName == "cr371_acc")
            {
                if (context.Depth > 1)
                {
                    tracingService.Trace("Exiting plugin execution due to depth check.");
                    return;
                }

                
                int oldCustomerType = 0;
                if (context.PreEntityImages.Contains("PreImage") && context.PreEntityImages["PreImage"] is Entity preImage)
                {
                    oldCustomerType = preImage.Contains("cr371_address1_freighttermscode") ? preImage.GetAttributeValue<OptionSetValue>("cr371_address1_freighttermscode").Value : 0;
                    tracingService.Trace($"Old Customer Type: {oldCustomerType}");
                }

               
                int newCustomerType = 0;
                if (context.PostEntityImages.Contains("PostImage") && context.PostEntityImages["PostImage"] is Entity postImage)
                {
                    newCustomerType = postImage.Contains("cr371_address1_freighttermscode") ? postImage.GetAttributeValue<OptionSetValue>("cr371_address1_freighttermscode").Value : 0;
                    tracingService.Trace($"New Customer Type: {newCustomerType}");
                }

               
                Entity customerToUpdate = new Entity("cr371_acc") { Id = context.PrimaryEntityId };

                if (newCustomerType == 1) 
                {
                    customerToUpdate["cr371_money"] = new Money(5000);
                    tracingService.Trace("Updated card limit to 5000 for Basic customer.");
                }
                else 
                {
                    customerToUpdate["cr371_money"] = new Money(10000);
                    tracingService.Trace("Updated card limit to 10000 for Premium customer.");
                }

                service.Update(customerToUpdate);
                tracingService.Trace("Customer record updated successfully.");

               
                QueryExpression orderQuery = new QueryExpression("cr371_cuss")
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

                tracingService.Trace("Fetching related orders for customer.");
                EntityCollection relatedOrders = service.RetrieveMultiple(orderQuery);
                tracingService.Trace($"Total related orders found: {relatedOrders.Entities.Count}");

                foreach (Entity order in relatedOrders.Entities)
                {
                    tracingService.Trace($"Updating order ID: {order.Id}");
                    Entity orderToUpdate = new Entity("cr371_cuss") { Id = order.Id };

                    if (newCustomerType == 1) 
                    {
                        orderToUpdate["cr371_creditlimit"] = new Money(10);
                        tracingService.Trace("Applied discount of 10 for Basic customer.");
                    }
                    else if (newCustomerType == 2) 
                    {
                        orderToUpdate["cr371_creditlimit"] = new Money(20);
                        tracingService.Trace("Applied discount of 20 for Premium customer.");
                    }

                    service.Update(orderToUpdate);
                    tracingService.Trace($"Order ID: {order.Id} updated successfully.");
                }

                tracingService.Trace("Plugin Execution Completed.");
            }
        }
    }
}
