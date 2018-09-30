using System.Threading;
using System.Threading.Tasks;
using Apprenticeship.AzureFunction.APIHelper;
using Apprenticeship.AzureFunction.Helper;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DSA.Function.Queue
{
    public static class ApprenticeshipQueueFunction
    {
        [FunctionName("ApprenticeshipQueueFunction")]
        public static async Task Run(
            [QueueTrigger("vacancysummaryqueueitem", Connection = "AzureWebJobsStorage")] string myQueueItem,
            TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {myQueueItem}");
            var cts = new CancellationToken();

            if (!string.IsNullOrWhiteSpace(myQueueItem))
            {
                //Lets Connect DSA ApprenticeshipFeed
                var response =
                    RestCosmosDbHelper.GetSfaPublicVacanySummaryRestHandle(ConnectionStringConstants.DSAFEEDRESTAPIBASEURI,
                        ConnectionStringConstants.DSAFEEDRESTREQUESTURI, 1, 100);
                var content = response.Content;
                log.Info($"Feed Response :{content}");
                //Lets Connect CosmosDB
               await RestCosmosDbHelper.GetConnectionHandleAsync(ConnectionStringConstants.URICOSMOSDB,
                    ConnectionStringConstants.PRIMARYKEYCOSMOSDB, null, cts);
               await  RestCosmosDbHelper.CreateCosmosDatabaseAsync(new Database() { Id = CosmosDbConstants.DatabaseName }, null, cts);
               await  RestCosmosDbHelper.CreateCollectionAsync(UriFactory.CreateDatabaseUri(CosmosDbConstants.DatabaseName),
                    new DocumentCollection { Id = CosmosDbConstants.DocumentCollectionName }, null, cts);
                var summaryCollection = RestCosmosDbHelper.GetSerializedResponse(response);
                foreach (var summary in summaryCollection)
                {
                   await RestCosmosDbHelper.CreateDocumentAsync(CosmosDbConstants.DatabaseName,
                        CosmosDbConstants.DocumentCollectionName, summary, null, cts);
                }
            }
        }
    }
}