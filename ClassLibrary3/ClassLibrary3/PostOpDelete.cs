using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace ClassLibrary3
{
    public class PostOpDelete : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            try
            {
                if (context.PrimaryEntityName == "cr371_acc" && context.MessageName.ToLower() == "delete")
                {
                    Guid deletedAccountId = context.PrimaryEntityId;
                    tracingService.Trace($"GUID received: {deletedAccountId}");

                    if (deletedAccountId == Guid.Empty)
                    {
                        tracingService.Trace("No valid GUID found. Exiting plugin.");
                        return;
                    }

                    Entity preImage = null;
                    if (context.PreEntityImages.Contains("PreImage") && context.PreEntityImages["PreImage"] is Entity)
                    {
                        preImage = context.PreEntityImages["PreImage"];
                    }

                    if (preImage != null && preImage.Contains("cr371_name"))
                    {
                        string deletedAccountName = preImage["cr371_name"].ToString();
                        tracingService.Trace($"Name of deleted user: {deletedAccountName}");
                    }
                    else
                    {
                        tracingService.Trace("PreImage does not contain 'cr371_name'.");
                    }

                    IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);

                    tracingService.Trace("Calling the delete function.");
                   // DeleteRelatedCussRecords(service, deletedAccountId, tracingService);
                    DeleteCussRecordsWithEmptyParent(service, tracingService);
                }
                else
                {
                    tracingService.Trace("Plugin not triggered on 'cr371_acc' delete event.");
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace($"Exception in plugin: {ex.Message}");
                throw new InvalidPluginExecutionException($"An error occurred in the PostOpDelete plugin: {ex.Message}");
            }
        }
        /*
        public void DeleteRelatedCussRecords(IOrganizationService service, Guid accountId, ITracingService tracingService)
        {
            try
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
                tracingService.Trace($"Number of related records found: {relatedRecords.Entities.Count}");

                foreach (Entity record in relatedRecords.Entities)
                {
                    try
                    {
                        service.Delete("cr371_cuss", record.Id);
                        tracingService.Trace($"Successfully deleted record: {record.Id}");
                    }
                    catch (Exception ex)
                    {
                        tracingService.Trace($"Error deleting record {record.Id}: {ex.Message}");
                        throw new InvalidPluginExecutionException($"Error deleting related records: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace($"Error retrieving related records: {ex.Message}");
                throw new InvalidPluginExecutionException($"Error retrieving related records: {ex.Message}");
            }
        }
        */
        public void DeleteCussRecordsWithEmptyParent(IOrganizationService service, ITracingService tracingService)
        {
            try
            {
                QueryExpression query = new QueryExpression("cr371_cuss")
                {
                    ColumnSet = new ColumnSet(false),
                    Criteria = new FilterExpression()
                    {
                        Conditions =
                        {
                            new ConditionExpression("cr371_parentcustomerid", ConditionOperator.Null)
                        }
                    }
                };

                EntityCollection orphanRecords = service.RetrieveMultiple(query);
                tracingService.Trace($"Number of orphan records found: {orphanRecords.Entities.Count}");

                foreach (Entity record in orphanRecords.Entities)
                {
                    try
                    {
                        service.Delete("cr371_cuss", record.Id);
                        tracingService.Trace($"Successfully deleted orphan record: {record.Id}");
                    }
                    catch (Exception ex)
                    {
                        tracingService.Trace($"Error deleting orphan record {record.Id}: {ex.Message}");
                        throw new InvalidPluginExecutionException($"Error deleting orphan records: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace($"Error retrieving orphan records: {ex.Message}");
                throw new InvalidPluginExecutionException($"Error retrieving orphan records: {ex.Message}");
            }
        }
    }
}
