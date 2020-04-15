using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace MyPlugins
{
    public class PreEntityImageDemo : IPlugin
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

            Entity lead = (Entity)context.InputParameters["Target"];

            try
            {
                    string modifiedBusinessPhone = lead.Attributes[LogicalName.leadBusinessPhoneField].ToString();

                    Entity preImage = (Entity)context.PreEntityImages["PreImage"];
                    string oldBusinessPhone = preImage.Attributes[LogicalName.leadBusinessPhoneField].ToString();

                    throw new InvalidPluginExecutionException("Phone number has been changed from " + oldBusinessPhone + " To" + modifiedBusinessPhone);


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
