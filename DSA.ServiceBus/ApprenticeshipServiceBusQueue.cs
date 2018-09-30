using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;

namespace DSA.ServiceBus
{
    public static class ApprenticeshipServiceBusQueue
    {
        [FunctionName("ApprenticeshipServiceBusQueue")]
        public static void Run([ServiceBusTrigger("myqueue", AccessRights.Manage, Connection = "AzureServiceBusQueue")]string myQueueItem, TraceWriter log)
        {
            log.Info($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
