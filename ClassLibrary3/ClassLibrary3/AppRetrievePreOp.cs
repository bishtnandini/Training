using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary3
{
    using DataContract;
    using Helper;
   public class AppRetrievePreOp : IPlugin
    {
        public readonly string unsecureValue;
        public readonly string secureValue;

        public AppRetrievePreOp (string unsecureConfig, string secureConfig)//{"name":"xyz"} 
        {
            if (string.IsNullOrWhiteSpace(unsecureConfig))
            {
                unsecureValue = string.Empty;
            }
            else
            {
                config data = JSONHelper.Deserialize<config>(unsecureConfig);
                unsecureValue = data.Name;
            }

            //secure
            if (string.IsNullOrWhiteSpace(secureConfig))
            {
                secureValue = string.Empty;
            }
            else
            {
                config data = JSONHelper.Deserialize<config>(secureConfig);
                secureValue = data.Name;
            }
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                if(context.Depth > 1)
                {
                    return;
                }

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is EntityReference)
                {
                    IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                    EntityReference applicationEntityRef = context.InputParameters["Target"] as EntityReference;

                    Entity updateApplication = new Entity(applicationEntityRef.LogicalName);
                    updateApplication.Id = applicationEntityRef.Id;
                    updateApplication["cr371_name"] = secureValue + " " + unsecureValue;
                    service.Update(updateApplication);

                    /*
                    //shared variable => shared the data between events of the plugin
                    //pre op 

                    Guid contactiid = service.Create(new Entity("contact"));
                    //setting the value of shared variable
                    context.SharedVariables["mycontactiid"] = contactiid.ToString();

                    //next event  access

                    Guid createdContactiid = Guid.Parse(context.SharedVariables["mycontactiid"].ToString());
                    */

                }
            }
            catch(Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
           
        }
    }
}
