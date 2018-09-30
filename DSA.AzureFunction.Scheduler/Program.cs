using System;
using System.Collections.Generic;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Management.Scheduler;
using Microsoft.WindowsAzure.Management.Scheduler.Models;
using Microsoft.WindowsAzure.Scheduler;
using Microsoft.WindowsAzure.Scheduler.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Queue.Protocol;
using Apprenticeship.AzureFunction.Helper;

namespace Apprenticeship.AzureFunction.Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            const string cloudServiceName = "sfademoarmbasedschedulerbeta1";
            const string jobCollectionName = "sfademoarmbasedjobcollectionbeta1";
            const string geoRegion = "west europe";
            // var AzureStorage = new ApprenticeShipSchedular();
            SchedulerManagementClient schedulerServiceClient;

            ArmSfaScheduler.SetPublishSettingsProperties();
            var credentials = ArmSfaScheduler.FromPublishSettingsFile(ArmSfaScheduler.PublishSettingFilePath,
                ArmSfaScheduler.SubscriptionName);
            Console.WriteLine("Executing CreateCloudClient\n\n");
            AzureSchedulerHelper.CreateCloudClient(cloudServiceName,geoRegion,credentials);
            Console.WriteLine("Executing RegisterScheduleResourceProvider\n\n");
            AzureSchedulerHelper.RegisterScheduleResourceProvider(credentials);
            Console.WriteLine("Executing QueryRegisteredSchedulerProperties\n\n");
            AzureSchedulerHelper.QueryRegisteredSchedulerProperties(credentials);
            Console.WriteLine("Executing CreateJobCollections\n\n");
            AzureSchedulerHelper.CreateJobCollections(cloudServiceName,jobCollectionName,credentials);
            Console.WriteLine("Executing CreateHttpActionBasedJob \n\n");
            AzureSchedulerHelper.CreateHttpActionBasedJob(cloudServiceName,jobCollectionName,credentials);
            Console.WriteLine("Executing CreateQueueBasedSchedulerJob\n");
            AzureSchedulerHelper.CreateQueueBasedSchedulerJob(cloudServiceName,jobCollectionName,credentials);
            Console.WriteLine("Executing GetSchedulerJobHistory \n\n");
            AzureSchedulerHelper.GetSchedulerJobHistory(cloudServiceName,jobCollectionName,credentials);

            Console.WriteLine("End of Creation ARM Scheduler\n\n");
            Console.ReadLine();
        }

    }
}