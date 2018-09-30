using System;
using System.Collections.Generic;
using System.Net;
using Apprenticeship.AzureFunction.Helper;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Management.Scheduler;
using Microsoft.WindowsAzure.Management.Scheduler.Models;
using Microsoft.WindowsAzure.Scheduler;
using Microsoft.WindowsAzure.Scheduler.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Queue.Protocol;
using System.Runtime.Serialization;
using Hyak.Common;

namespace Apprenticeship.AzureFunction.Scheduler
{
    [DataContract]
   public sealed class AzureSchedulerHelper
    {
        public static void CreateCloudClient(string cloudServiceName,string geoRegion,CertificateCloudCredentials credentials)
        {
            try
            {
            var cloudServiceClient = new CloudServiceManagementClient(credentials);
                cloudServiceClient.CloudServices.Delete(cloudServiceName);
            var result = cloudServiceClient.CloudServices.Create(cloudServiceName, new CloudServiceCreateParameters()
            {
                Description = cloudServiceName,
                GeoRegion = geoRegion,
                Label = cloudServiceName
            });

            Console.WriteLine("CreateCloudClient :\n" + result.Status);
            Console.WriteLine("CreateCloudClient :\n" + result.HttpStatusCode);
            }
            catch (Exception e)
            {
                Console.WriteLine("CreateCloudClient Exception :\n" +e.Message);
                throw;
            }
        }
        public static void RegisterScheduleResourceProvider(CertificateCloudCredentials credentials)
        {
            var schedulerServiceClient = new SchedulerManagementClient(credentials);
            try
            {
            var result2 = schedulerServiceClient.RegisterResourceProvider();

            Console.WriteLine("RegisterScheduleResourceProvider :\n" + result2.RequestId);
            Console.WriteLine("RegisterScheduleResourceProvider :\n" + result2.StatusCode);
            Console.ReadLine();
            }
            catch (CloudException exc)
            {
                if (exc.Response.StatusCode != HttpStatusCode.Conflict)
                {
                    Console.WriteLine("RegisterScheduleResourceProvider Exception :\n"+exc.Message);
                    throw;
                }

            }
        }
        public static void QueryRegisteredSchedulerProperties(CertificateCloudCredentials credentials)
        {
            try
            {
                var schedulerServiceClient = new SchedulerManagementClient(credentials);
                var result3 = schedulerServiceClient.GetResourceProviderProperties();
                foreach (var prop in result3.Properties)
                {
                    Console.WriteLine("QueryRegisteredSchedulerProperties :\n" + prop.Key + ": " + prop.Value);
                }
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("QueryRegisteredSchedulerProperties Exception :\n"+e.Message);
                throw;
            }
        }
        public static void CreateJobCollections(string cloudServiceName,string jobCollectionName,CertificateCloudCredentials credentials)
        {
            try
            {
                var schedulerServiceClient = new SchedulerManagementClient(credentials);
              //  SchedulerOperationStatusResponse deletejob=schedulerServiceClient.JobCollections.Delete(cloudServiceName, jobCollectionName);
               JobCollectionCheckNameAvailabilityResponse response= schedulerServiceClient.JobCollections.CheckNameAvailability(cloudServiceName, jobCollectionName);
                if (response.IsAvailable == true)
                {
                    var result4 = schedulerServiceClient.JobCollections.Create(cloudServiceName, jobCollectionName,
                        new JobCollectionCreateParameters()
                        {
                            Label = jobCollectionName,
                            IntrinsicSettings = new JobCollectionIntrinsicSettings()
                            {
                                Plan = JobCollectionPlan.Standard,

                                Quota = new JobCollectionQuota()
                                {
                                    MaxJobCount = 100,
                                    MaxJobOccurrence = 100,
                                    MaxRecurrence = new JobCollectionMaxRecurrence()
                                    {
                                        Frequency = JobCollectionRecurrenceFrequency.Minute,
                                        Interval = 1
                                    }
                                }
                            }
                        });

                    Console.WriteLine("CreateJobCollections :\n" + result4.RequestId);
                    Console.WriteLine("CreateJobCollections :\n" + result4.StatusCode);
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateJobCollections Exception :\n"+ex.Message);
            }
        }
        public static void CreateHttpActionBasedJob(string cloudServiceName,string jobCollectionName,CertificateCloudCredentials credentials)
        {
            try
            {
                var schedulerClient = new SchedulerClient(cloudServiceName, jobCollectionName, credentials);
                var result5 = schedulerClient.Jobs.Create(new JobCreateParameters()
                {
                    Action = new JobAction()
                    {
                        Type = JobActionType.Http,
                        Request = new JobHttpRequest()
                        {
                            Body = "fetchApprenticeshipFeed=sfaarmshcedulertest&command=fetchdata",
                            Headers = new Dictionary<string, string>()
                            {
                                {"Content-Type", "application/x-www-form-urlencoded"},
                                {"x-something", "value123"}
                            },
                            Method = "POST",
                            Uri = new Uri("http://postcatcher.in/catchers/527af9acfe325802000001cb")
                        }
                    },
                    StartTime = DateTime.UtcNow,
                    Recurrence = new JobRecurrence()
                    {
                        Frequency = JobRecurrenceFrequency.Minute,
                        Interval = 1,
                        Count = 5
                    }
                });

                Console.WriteLine("CreateHttpActionBasedJob :\n" + result5.RequestId);
                Console.WriteLine("CreateHttpActionBasedJob :\n" + result5.StatusCode);
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("CreateHttpActionBasedJob :\n" + e.Message);
            }
        }
        public static void CreateQueueBasedSchedulerJob(string cloudServiceName,string jobCollectionName,CertificateCloudCredentials credentials)
        {
            try
            {
                var storageAccount =
                    new CloudStorageAccount(
                        new StorageCredentials(ConnectionStringConstants.STORAGEACCOUNTNAME,
                            ConnectionStringConstants.STORAGEKEYNAME), true);
                var queueClient = storageAccount.CreateCloudQueueClient();
                var queue = queueClient.GetQueueReference(ConnectionStringConstants.STORAGEQUEUENAME);
                queue.CreateIfNotExists();

                var perm = new QueuePermissions();
                var policy = new SharedAccessQueuePolicy
                {
                    SharedAccessExpiryTime = DateTime.MaxValue,
                    Permissions = SharedAccessQueuePermissions.Add
                };
                perm.SharedAccessPolicies.Add("jobcoll001policy", policy);

                queue.SetPermissions(perm);
                var sas = queue.GetSharedAccessSignature(new SharedAccessQueuePolicy(), "jobcoll001policy");

                var schedulerClient = new SchedulerClient(cloudServiceName, jobCollectionName, credentials);
                var result = schedulerClient.Jobs.Create(new JobCreateParameters()
                {
                    Action = new JobAction()
                    {
                        Type = JobActionType.StorageQueue,
                        QueueMessage = new JobQueueMessage()
                        {
                            Message = "hello there!",
                            QueueName = ConnectionStringConstants.STORAGEQUEUENAME,
                            SasToken = sas,
                            StorageAccountName = ConnectionStringConstants.STORAGEACCOUNTNAME
                        }
                    },
                    StartTime = DateTime.UtcNow,
                    Recurrence = new JobRecurrence()
                    {
                        Frequency = JobRecurrenceFrequency.Minute,
                        Interval = 1,
                        Count = 5
                    }
                });

                Console.WriteLine("CreateQueueBasedSchedulerJob :\n" + result.RequestId);
                Console.WriteLine("CreateQueueBasedSchedulerJob :\n" + result.StatusCode);
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("CreateQueueBasedSchedulerJob :\n" + e.Message);
            }
        }
        public static void GetSchedulerJobHistory(string cloudServiceName,string jobCollectionName,CertificateCloudCredentials credentials)
        {
            try
            {
                var schedulerClient = new SchedulerClient(cloudServiceName, jobCollectionName, credentials);
                foreach (var job in schedulerClient.Jobs.List(
                    new JobListParameters() {Top = 100 /*State = JobState.Enabled*/}))
                {
                    Console.WriteLine("GetSchedulerJobHistory :\n" + "Job: {0} - Action: {1} - State: {2} - Status: {3}",
                        job.Id, job.Action, job.State,
                        job.Status);

                    foreach (var history in schedulerClient.Jobs.GetHistory(job.Id, new JobGetHistoryParameters()))
                    {
                        Console.WriteLine("GetSchedulerJobHistory : - > {0} - {1}: {2} \n\n", history.StartTime,
                            history.EndTime, history.Message);
                    }
                }

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("GetSchedulerJobHistory :\n" + e.Message);
            }
        }
    }
}