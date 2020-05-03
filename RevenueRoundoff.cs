using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace MyPlugins
{
    public class RevenueRoundoff : IPlugin
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

            Entity account = (Entity)context.InputParameters["Target"];

            try
            {
                    if(account.Attributes[LogicalName.accountRevenueField]!= null) // if update to a blank field

                    {
                        decimal revenue = ((Money)account.Attributes[LogicalName.accountRevenueField]).Value;
                        revenue = Math.Round(revenue, 2);
                    }

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
