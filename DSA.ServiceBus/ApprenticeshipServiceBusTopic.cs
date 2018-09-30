using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;

namespace DSA.ServiceBus
{
    public static class ApprenticeshipServiceBusTopic
    {
        [FunctionName("ApprenticeshipServiceBusTopic")]
        public static void Run([ServiceBusTrigger("CosmosDBAddUpdate", "mysubscription", AccessRights.Manage, Connection = "AzureServiceBusTopic")]string mySbMsg, TraceWriter log)
        {
            log.Info($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
