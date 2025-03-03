using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace ClassLibrary3
{
    public class PreOpeCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the execution context
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Ensure the context contains a target entity (the record being created)
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity entity)
            {
                // Check if the entity contains the 'country' OptionSet field
                if (entity.Contains("cr371_country") && entity["cr371_country"] is OptionSetValue)
                {
                    OptionSetValue countryOption = (OptionSetValue)entity["cr371_country"];

                    // Check if the country OptionSet value is 2 (assuming 2 represents India)
                    if (countryOption.Value == 2)
                    {
                        // Check if the mobile number field exists and is a string
                        if (entity.Contains("cr371_mobileno") && entity["cr371_mobileno"] is string)
                        {
                            string mobileNumber = (string)entity["cr371_mobileno"];

                            // Append +91 if it's not already there
                            if (!mobileNumber.StartsWith("+91"))
                            {
                                entity["cr371_mobileno"] = "+91" + mobileNumber;
                                context.InputParameters["Target"] = entity;  // Ensure the modification is applied
                            }
                        }
                    }
                }
            }
        }
    }
}

