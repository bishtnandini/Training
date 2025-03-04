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

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Ensure the "Target" entity is present
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity entity)
            {
                // Validate if "cr371_country" is present
                if (!entity.Contains("cr371_country") || !(entity["cr371_country"] is OptionSetValue countryOption))
                {
                    throw new InvalidPluginExecutionException("Please select a country before saving the record.");
                }

                int countryValue = countryOption.Value;
                string requiredPrefix = string.Empty;

                // Assign prefix based on country
                switch (countryValue)
                {
                    case 1:
                        requiredPrefix = "+96";
                        break;
                    case 2:
                        requiredPrefix = "+91";
                        break;
                    case 3:
                        requiredPrefix = "+88";
                        break;
                    default:
                        throw new InvalidPluginExecutionException("Invalid country selection.");
                }

                // Check if "cr371_mobileno" exists and is not null or empty
                if (!entity.Contains("cr371_mobileno") || entity["cr371_mobileno"] == null || string.IsNullOrWhiteSpace(entity["cr371_mobileno"].ToString()))
                {
                    throw new InvalidPluginExecutionException("Mobile number cannot be empty. Please provide a valid mobile number.");
                }

                string mobileNumber = entity["cr371_mobileno"].ToString();

               
                if (!mobileNumber.StartsWith(requiredPrefix))
                {
                    
                    mobileNumber = requiredPrefix + mobileNumber;

                   
                    entity["cr371_mobileno"] = mobileNumber;
                }
            }
        }
    }
}

