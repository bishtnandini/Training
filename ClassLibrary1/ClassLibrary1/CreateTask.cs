using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace for D365 interaction
using Microsoft.Xrm.Sdk;
using System.ServiceModel;



namespace ClassLibrary1
{
    public class CreateTask : IPlugin

    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                //obtain the target entity from the input parameter
                Entity entity = (Entity)context.InputParameters["Target"];

                //if not this plugin was not registered correctly.
                if (entity.LogicalName != "account")
                    return;
                try
                {
                    //create a record task activity to follow up
                    Entity followup = new Entity("task");

                    followup["subject"] = "Send email to the new customer.";
                    followup["description"] = "follow up with the customer ";
                    followup["scheduledstart"] = DateTime.Now.AddDays(7);
                    followup["scheduledend"] = DateTime.Now.AddDays(7);
                    followup["category"] = context.PrimaryEntityName;

                    //refer to the lead in the task activity
                    if (context.OutputParameters.Contains("id"))
                    {
                        Guid regardingobjectid = new Guid(context.OutputParameters["id"].ToString());
                        string regardingobjectidType = "account";

                        followup["regardingobjectid"] =
                            new EntityReference(regardingobjectidType, regardingobjectid);

                    }

                    //obtain the origanization service reference
                    IOrganizationServiceFactory serviceFactory =
                        (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    IOrganizationService service =
                        serviceFactory.CreateOrganizationService(context.UserId);

                    service.Create(followup);

                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("an error occur", ex);
                }


            }
        }

    }
}
