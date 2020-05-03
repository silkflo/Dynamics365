using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace MyPlugins
{
    public class UpdateHelloWorld :IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Extract the tracing service for use in debugging sandboxed plug-ins.  
            // If you are not registering the plug-in in the sandbox, then you do  
            // not have to add any tracing service related code.  
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the organization service reference which you will need for  
            // web service calls.  
            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            


            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity contact = (Entity)context.InputParameters["Target"];

                if (contact.LogicalName != "contact")
                    return;

                try
                {
                    //string firstNameValue = string.Empty;
                //    string key = context.SharedVariables["key1"].ToString();

                    string lastNameValue = contact.GetAttributeValue<string>(LogicalName.contactLastNameField);
                    string firstNameValue = contact.GetAttributeValue<string>(LogicalName.contactFirstNameField);

                    Entity preImage = (Entity)context.PreEntityImages["PreImage"];
                    string oldLastNameValue = preImage.Attributes[LogicalName.contactLastNameField].ToString();
                    string oldFirstNameValue = string.Empty;
                    if (preImage.Attributes.Contains(LogicalName.contactFirstNameField))
                    {
                        oldFirstNameValue = preImage.Attributes[LogicalName.contactFirstNameField].ToString();
                    }

                    // string lastNameValue = (string)contact["lastname"];

                    if (contact.Attributes.Contains(LogicalName.contactLastNameField))
                    {
                        lastNameValue = contact.Attributes[LogicalName.contactLastNameField].ToString();
                    }
                    else
                    {
                        lastNameValue = oldLastNameValue;
                    }
                        

                    // read form attribute
                    if (contact.Attributes.Contains(LogicalName.contactFirstNameField))
                    {
                        firstNameValue = contact.Attributes[LogicalName.contactFirstNameField].ToString();
                    }
                    else
                    {
                        firstNameValue = oldFirstNameValue;
                    }

                       //write data to Atribute
                        contact.Attributes.Add(LogicalName.contactDescriptionField, "Hello " + firstNameValue + " " + lastNameValue);

                 //   throw new InvalidPluginExecutionException(key);
                }



                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in MyPlug-in.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("MyPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}
