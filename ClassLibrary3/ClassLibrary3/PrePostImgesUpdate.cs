using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace ClassLibrary3
{
    public class PrePostImgesUpdate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);

            if (context.PrimaryEntityName == "cr371_acc")
            {
                if (context.Depth > 1) return;

                Entity preImage = null;
                Entity postImage = null;

                // Check if Pre-Image exists
                if (context.PreEntityImages.Contains("PreImage") && context.PreEntityImages["PreImage"] is Entity)
                {
                    preImage = context.PreEntityImages["PreImage"];
                }

                // Check if Post-Image exists
                if (context.PostEntityImages.Contains("PostImage") && context.PostEntityImages["PostImage"] is Entity)
                {
                    postImage = context.PostEntityImages["PostImage"];
                }

                // Retrieve current record
                Entity accRecord = service.Retrieve("cr371_acc", context.PrimaryEntityId, new ColumnSet("cr371_money", "cr371_address1_freighttermscode", "cr371_address1_addresstypecode"));

                Money moneyValue = accRecord.Contains("cr371_money") ? accRecord.GetAttributeValue<Money>("cr371_money") : new Money(0);
                int address = accRecord.Contains("cr371_address1_freighttermscode") ? accRecord.GetAttributeValue<OptionSetValue>("cr371_address1_freighttermscode").Value : 1;
                int addressCode = accRecord.Contains("cr371_address1_addresstypecode") ? accRecord.GetAttributeValue<OptionSetValue>("cr371_address1_addresstypecode").Value : 1;

                // Use Pre-Image data (Example: Checking previous value of money)
                Money preMoney = preImage != null && preImage.Contains("cr371_money") ? preImage.GetAttributeValue<Money>("cr371_money") : new Money(0);

                // Update Account money field
                Entity updateAcc = new Entity("cr371_acc");
                updateAcc.Id = context.PrimaryEntityId;
                updateAcc["cr371_money"] = (address == 1 && addressCode == 1) ? new Money(500) : new Money(600);

                service.Update(updateAcc);

                Entity updatedAccRecord = service.Retrieve("cr371_acc", context.PrimaryEntityId, new ColumnSet("cr371_money"));
                Money upMoney = updatedAccRecord.GetAttributeValue<Money>("cr371_money");

                // Retrieve all related Customer (cr371_cuss) records using the lookup field
                QueryExpression query = new QueryExpression("cr371_cuss")
                {
                    ColumnSet = new ColumnSet("cr371_creditlimit"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                {
                    new ConditionExpression("cr371_parentcustomerid", ConditionOperator.Equal, context.PrimaryEntityId)
                }
                    }
                };

                EntityCollection relatedCustomers = service.RetrieveMultiple(query);

                // Update cr371_creditlimit for each related cr371_cuss record
                foreach (Entity customer in relatedCustomers.Entities)
                {
                    Entity updateCustomer = new Entity("cr371_cuss");
                    updateCustomer.Id = customer.Id;
                    updateCustomer["cr371_creditlimit"] = new Money(upMoney.Value * 1.2M); // Example: Setting credit limit as 1.2x of money
                    service.Update(updateCustomer);
                }
            }
        }
    }
    }
