using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace MyPlugins 
{
    public class AccountCreate : IPlugin
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

              

                try
                {
                    Entity accountRecord = new Entity("account");
                   
                    Random random = new Random();

                    //read value
                    string lastNameValue = contact.Attributes[LogicalName.contactLastNameField].ToString();

                    //single line of text
                    accountRecord.Attributes.Add(LogicalName.accountNameField, lastNameValue + " Compagny");

                    //Multiple Lines of Text
                    accountRecord.Attributes.Add(LogicalName.accountDescriptionField, "this is an account automatically generated.\r\nFor more details please contact your administrator.");

                    //Option Set
                    int randomIndustry = random.Next(1, 34);
                    accountRecord.Attributes.Add(LogicalName.accountIndustryField, new OptionSetValue(randomIndustry));

                    //MultiSelect Option Set V1
                    int randomSoftware1 = random.Next(100000000, 100000005);
                    int randomSoftware2 = random.Next(100000000, 100000005);

                    while (randomSoftware2 == randomSoftware1)
                    randomSoftware2 = random.Next(100000000, 100000005);

                    OptionSetValueCollection OPcollection = new OptionSetValueCollection() { new OptionSetValue(randomSoftware1), new OptionSetValue(randomSoftware2) };
                    accountRecord.Attributes[LogicalName.accountSoftwareField] = OPcollection;

                    //MultiSelect option set V2
                    accountRecord.Attributes.Add(LogicalName.accountLanguageField, new OptionSetValueCollection() { new OptionSetValue(100000001) });

                    //Two Options
                    accountRecord.Attributes.Add(LogicalName.accountEmailAllowField, true); //first value is 0 then false. 2nd value is 1 then true

                    //Whole Number
                    int randomEmployeeAmount = random.Next(0, 100001);
                    accountRecord.Attributes.Add(LogicalName.accountEmployeeQtyField, randomEmployeeAmount);

                    //Floating Point Number
                    double randomLenght = random.NextDouble()*1000;
                    accountRecord.Attributes.Add(LogicalName.accountLenghtField, randomLenght);

                    //Decimal Number
                    int randomNumber = random.Next(-1000000, 1000000);
                     while (randomNumber == 0)
                     {
                         randomNumber = random.Next(-1, 1);
                     }
                      
                    double randomDecimal = random.NextDouble() * randomNumber;
                    accountRecord.Attributes.Add(LogicalName.accountDecimalField,(decimal)randomDecimal);

                    //Currency
                    decimal creditValue = 50;
                    Money creditLimit = new Money(creditValue);
                    accountRecord.Attributes.Add(LogicalName.accountCreditLimitField, creditLimit);

                    //Currency V2
                    accountRecord.Attributes[LogicalName.accountMoneyValueField] = creditLimit;

                    //Date and Time
                    //accountRecord.Attributes.Add(LogicalName.accountDateField, DateTime.Today.AddYears(1));

                    DateTime nextYear = DateTime.Today.AddYears(1);
                    accountRecord.Attributes[LogicalName.accountDateField] = nextYear;

                    //Image



                    //Lookup

                   

                    //Customer
                    accountRecord.Attributes.Add(LogicalName.accountPrimarryContact, contact.ToEntityReference());



                    Guid accountGUID = service.Create(accountRecord);
                  

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
