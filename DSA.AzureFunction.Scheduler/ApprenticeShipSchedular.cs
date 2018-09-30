using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Management.Scheduler;
using Microsoft.WindowsAzure.Management.Scheduler.Models;
using Microsoft.WindowsAzure.Scheduler;
using Microsoft.WindowsAzure.Scheduler.Models;

namespace Apprenticeship.AzureFunction.Scheduler
{
    // https://msdn.microsoft.com/en-us/library/dn722415.aspx

    [Obsolete("Not used anymore", true)]
    public class ApprenticeShipSchedular
    {
        // retrieve the windows azure managment certificate from current user
        public ApprenticeShipSchedular()
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            var certificate = store.Certificates.Find(X509FindType.FindByThumbprint, "CA4716F85EC0962E2B69BB7D626CC16B924E550D", false)[0];
            store.Close();
            //
            // create management credencials and cloud service management client
            var credentials = new CertificateCloudCredentials("bd27a79c-de25-4097-a874-3bb35f2b926a", certificate);
            var cloudServiceMgmCli = new CloudServiceManagementClient(credentials);

            // create cloud service
            var cloudServiceCreateParameters = new CloudServiceCreateParameters
            {
                Description = "sfaarmscheduler",
                Email = "dinesh.tripathi@openenergi.com",
                GeoRegion = "West Europe",
                Label = "sfaarmscheduler"
            };
            var cloudService = cloudServiceMgmCli.CloudServices.Create("sfaarmscheduler", cloudServiceCreateParameters);
            // create job collection
            var schedulerMgmCli = new SchedulerManagementClient(credentials);
            var jobCollectionIntrinsicSettings = new JobCollectionIntrinsicSettings
            {
                //Add Credentials for X.509
                Plan = JobCollectionPlan.Free,
                Quota = new JobCollectionQuota
                {
                    MaxJobCount = 5,
                    MaxJobOccurrence = 1,
                    MaxRecurrence = new JobCollectionMaxRecurrence
                    {
                        Frequency = JobCollectionRecurrenceFrequency.Hour,
                        Interval = 1
                    }
                }
            };
            var jobCollectionCreateParameters = new JobCollectionCreateParameters
            {
                IntrinsicSettings = jobCollectionIntrinsicSettings,
                Label = "sfajobcollection"
            };
            var jobCollectionCreateResponse =
                schedulerMgmCli.JobCollections.Create("sfaarmscheduler", "sfajobcollection", jobCollectionCreateParameters);

            var schedulerClient = new SchedulerClient("sfaarmscheduler", "sfajobcollection", credentials);
            var jobAction = new JobAction
            {
                Type = JobActionType.StorageQueue,
                QueueMessage = new JobQueueMessage()
                { Message = "test", QueueName = "vacancysummaryqueueitem",SasToken = "?sv=2017-04-17&ss=bfqt&srt=sco&sp=rwdlacup&se=2017-09-01T21:58:06Z&st=2017-09-01T13:58:06Z&spr=https&sig=r0u1A01ytcN213lsWO47Td7DUaU7lo3aQTPNTCrHRxU%3D", StorageAccountName = "sfabetastorage" }
                //Request = new JobHttpRequest
                //{
                //    Uri = new Uri("http://blog.shaunxu.me"),
                //    Method = "GET"
                //}
            };
            var jobRecurrence = new JobRecurrence
            {
                Frequency = JobRecurrenceFrequency.Hour,
                Interval = 1
            };
            var jobCreateOrUpdateParameters = new JobCreateOrUpdateParameters
            {
                Action = jobAction,
                Recurrence = jobRecurrence
            };
            var jobCreateResponse = schedulerClient.Jobs.CreateOrUpdate("poll_blog", jobCreateOrUpdateParameters);

            var jobGetHistoryParameters = new JobGetHistoryParameters
            {
                Skip = 0,
                Top = 100
            };
            var history = schedulerClient.Jobs.GetHistory("poll_blog", jobGetHistoryParameters);
            foreach (var action in history)
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", action.Status, action.Message, action.RetryCount,
                    action.RepeatCount, action.Timestamp);
        }
    }
}