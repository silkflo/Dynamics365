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
                    /*
                    It is  an infinite calling the method. For example
                    You have a workflow(trigger by when status field change) in which an account is updated. 
                    You have account plugin and it is configured as "update" message post operation. 
                    In that plugin you did update the account status.
                    In the above what happens, if the workflow trigger first workflow and then call plugin and then from plugin it call same workflow.
                    So it continuously looping.
                    To avoid this CRM by default they given how much looped it went, so you use "context.depth >1 return" in plugin code.
                    */

                    tracingService.Trace(context.Depth.ToString());
                    if (context.Depth > 1) //if depth >1 means infinity look, update trigger by a non human
                        return;            //terminates execution of the method

                    if (account.Attributes[LogicalName.accountRevenueField]!= null) // if update to a blank field

                    {
                        decimal revenue = ((Money)account.Attributes[LogicalName.accountRevenueField]).Value;
                        revenue = Math.Round(revenue, 1);

                        account.Attributes[LogicalName.accountRevenueField] = new Money(revenue);
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
