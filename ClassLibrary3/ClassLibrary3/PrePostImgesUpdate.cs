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
            tracingService.Trace("PrePostImagesUpdate Plugin Execution Started.");

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);

            if (context.PrimaryEntityName == "cr371_acc")
            {
                if (context.Depth > 1)
                {
                    tracingService.Trace("Execution stopped: Depth > 1 to prevent infinite loop.");
                    return;
                }

                // Retrieve Pre-Image (Before Update Values)
                Entity preImage = context.PreEntityImages.Contains("PreImage") ? context.PreEntityImages["PreImage"] : null;

                // Retrieve Post-Image (After Update Values)
                Entity postImage = context.PostEntityImages.Contains("PostImage") ? context.PostEntityImages["PostImage"] : null;

                // Fetch the updated record from the context (Current Update Values)
                Entity accRecord = service.Retrieve("cr371_acc", context.PrimaryEntityId, new ColumnSet("cr371_money", "cr371_address1_freighttermscode", "cr371_address1_addresstypecode"));

                // Retrieve Money value from Pre-Image (Old Value)
                Money preMoneyValue = preImage != null && preImage.Contains("cr371_money")
                    ? preImage.GetAttributeValue<Money>("cr371_money")
                    : new Money(0);

                // Retrieve Money value from Post-Image (New Value after Update)
                Money postMoneyValue = postImage != null && postImage.Contains("cr371_money")
                    ? postImage.GetAttributeValue<Money>("cr371_money")
                    : new Money(0);

                tracingService.Trace("Pre-Money Value: {0}, Post-Money Value: {1}", preMoneyValue.Value, postMoneyValue.Value);

                // Retrieve Address fields
                int address = accRecord.Contains("cr371_address1_freighttermscode")
                    ? accRecord.GetAttributeValue<OptionSetValue>("cr371_address1_freighttermscode").Value
                    : 1;

                int addressCode = accRecord.Contains("cr371_address1_addresstypecode")
                    ? accRecord.GetAttributeValue<OptionSetValue>("cr371_address1_addresstypecode").Value
                    : 1;

                tracingService.Trace("Address: {0}, Address Code: {1}", address, addressCode);

                // Decision Based on Address Type and Freight Terms
                Money updatedMoney = (address == 1 && addressCode == 1) ? new Money(500) : new Money(600);

                // Update Account money field
                Entity updateAcc = new Entity("cr371_acc");
                updateAcc.Id = context.PrimaryEntityId;
                updateAcc["cr371_money"] = updatedMoney;
                service.Update(updateAcc);
                postMoneyValue = updatedMoney;

                tracingService.Trace("Updated Account Money: {0}", updatedMoney.Value);

                // Retrieve related Customer (cr371_cuss) records using lookup field
                QueryExpression query = new QueryExpression("cr371_cuss")
                {
                    ColumnSet = new ColumnSet("cr371_creditlimit", "cr371_parentcustomerid"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression("cr371_parentcustomerid", ConditionOperator.Equal, context.PrimaryEntityId)
                        }
                    }
                };

                EntityCollection relatedCustomers = service.RetrieveMultiple(query);
                tracingService.Trace("Number of related customers retrieved: {0}", relatedCustomers.Entities.Count);

                // Update cr371_creditlimit for each related cr371_cuss record
                foreach (Entity customer in relatedCustomers.Entities)
                {
                    Money currentCreditLimit = customer.Contains("cr371_creditlimit")
                        ? customer.GetAttributeValue<Money>("cr371_creditlimit")
                        : new Money(0);

                    tracingService.Trace("Customer {0} current credit limit: {1}", customer.Id, currentCreditLimit.Value);

                    if (currentCreditLimit.Value == 100 || currentCreditLimit.Value < postMoneyValue.Value * 1.2M)
                    {
                        Entity updateCustomer = new Entity("cr371_cuss");
                        updateCustomer.Id = customer.Id;
                        updateCustomer["cr371_creditlimit"] = new Money(postMoneyValue.Value * 1.2M);
                        service.Update(updateCustomer);
                        tracingService.Trace("Updated customer {0} credit limit to: {1}", customer.Id, postMoneyValue.Value * 1.2M);
                    }
                }
            }
        }
    }
}
