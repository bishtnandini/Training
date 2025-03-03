using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace ClassLibrary3
{
   public  class PreOpUpdate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Ensure the context contains a target entity (the record being updated)
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity entity)
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                //Capitalize Name Field
                if (entity.Contains("cr371_name") && entity["cr371_name"] is string name)
                {
                    entity["cr371_name"] = textInfo.ToTitleCase(name.ToLower());
                }

                //Automatically update Last Modified Date
                entity["modifiedon"] = DateTime.UtcNow;

               
            }
        }
    }
}
