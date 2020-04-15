using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace MyPlugins
{
    public class CreateHelloWorld : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {

        ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));

        IPluginExecutionContext context = (IPluginExecutionContext)
            serviceProvider.GetService(typeof(IPluginExecutionContext));

        IOrganizationServiceFactory serviceFactory =
            (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
        IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);


        if (context.InputParameters.Contains("Target") &&
            context.InputParameters["Target"] is Entity)
        {

            Entity contact = (Entity)context.InputParameters["Target"];

            try
            {

                    string firstNameValue = string.Empty;


                    if (contact.Attributes.Contains(LogicalName.contactFirstNameField))
                    {
                        firstNameValue = contact.Attributes[LogicalName.contactFirstNameField].ToString();
                    }

                    string lastNameValue = contact.Attributes[LogicalName.contactLastNameField].ToString();


                    //write data to Atribute
                    contact.Attributes.Add(LogicalName.contactDescriptionField, "Hello " + firstNameValue + " " + lastNameValue);

                    //finish the code editing description field on update


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
