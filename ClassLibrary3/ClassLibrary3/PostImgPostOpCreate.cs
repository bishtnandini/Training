using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace ClassLibrary3
{
   public class PostImgPostOpCreate :IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

         
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);

            
            if (context.PrimaryEntityName == "cr371_acc" && context.MessageName == "Create")
            {
                
                Entity postImage = null;
                if (context.PostEntityImages.Contains("PostImage") && context.PostEntityImages["PostImage"] is Entity)
                {
                    postImage = (Entity)context.PostEntityImages["PostImage"];
                }

                if (postImage != null)
                {
                    // Extract values from the Post Image
                    string accountName = postImage.GetAttributeValue<string>("cr371_name");
                    string phoneNumber = postImage.GetAttributeValue<string>("cr371_mobileno");
                    Money money = postImage.GetAttributeValue<Money>("cr371_money");
                    int oldCustomerType = 0;
                   
                   oldCustomerType = postImage.Contains("cr371_address1_freighttermscode") ? postImage.GetAttributeValue<OptionSetValue>("cr371_address1_freighttermscode").Value : 0;

                    Entity updateAcc = new Entity("cr371_acc");
                    updateAcc.Id = context.PrimaryEntityId;
                    updateAcc["cr371_money"] = (oldCustomerType == 1 ) ? new Money(5000) : new Money(10000);
                    service.Update(updateAcc);


                    Entity contactRecord = new Entity("cr371_cuss");

                    if (oldCustomerType == 1)
                    {
                        contactRecord["cr371_creditlimit"] = new Money(5000);
                      
                    }
                    else
                    {
                        contactRecord["cr371_creditlimit"] = new Money(10000);
                       
                    }
                    contactRecord["cr371_fullname"] = accountName;
                    contactRecord["cr371_mobileno"] = phoneNumber;
                    contactRecord["cr371_parentcustomerid"] = new EntityReference("cr371_acc", context.PrimaryEntityId);
                    contactRecord["cr371_accountrolecode"] = new OptionSetValue(2);
                   // contactRecord["cr371_creditlimit"] = money.Value;
                    contactRecord["cr371_donotphone"] = true;
                    contactRecord["cr371_nmberofchildren"] = 0;

                    
                    Guid contactId = service.Create(contactRecord);
                }
            }
        }
    }
}
