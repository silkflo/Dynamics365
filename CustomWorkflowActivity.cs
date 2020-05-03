using System.Collections;
using System.Activities;
using System.ServiceModel;

// Microsoft Xrm DLLs
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;


namespace CustomWorkflowActivities
{

    public sealed partial class AddActivity : CodeActivity
    {
        // Define Input/Output Arguments
        [Input("First number")]
        public InArgument<int> firstNumber { get; set; }

        [Input("Second number")]
        public InArgument<int> secondNumber { get; set; }

        [Output("Result")]
        public OutArgument<int> result { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            int firstnumber = this.firstNumber.Get(executionContext);
            int secondNumber = this.secondNumber.Get(executionContext);
            // Retrieve the summands and perform addition

            int result = firstnumber + secondNumber;

            this.result.Set(executionContext, result);

        }

    }

    public sealed partial class CustomActivity : CodeActivity
    {
        /// <summary>
        /// Creates a task with a subject equal to the ID of the input EntityReference
        /// </summary>

        // Define Input/Output Arguments
        [RequiredArgument]
        [Input("Account Name")]
        [ReferenceTarget("account")]
        public InArgument<EntityReference> AccountName { get; set; }

        [Output("TaskCreated")]
        [ReferenceTarget("task")]
        public OutArgument<EntityReference> taskCreated { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory =
                executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service =
                serviceFactory.CreateOrganizationService(context.UserId);

            // Retrieve the id
            Guid accountId = this.AccountName.Get(executionContext).Id;

            // Create a task entity
            Entity task = new Entity();
            task.LogicalName = "task";
            task["subject"] = accountId.ToString();
            task["regardingobjectid"] = new EntityReference("account", accountId);
            Guid taskId = service.Create(task);
            this.taskCreated.Set(executionContext, new EntityReference("task", taskId));
        }


    }

}
//</snippetAddActivity>

