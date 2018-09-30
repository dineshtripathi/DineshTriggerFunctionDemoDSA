using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Apprenticeship.AzureFunction.APIHelper;
using Apprenticeship.AzureFunction.Helper;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace DSA.Function.HttpTrigger
{
    public static class ApprenticeshipTriggerFunction
    {
        [FunctionName("ApprenticeshipTriggerFunction")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => String.Compare(q.Key, "name", StringComparison.OrdinalIgnoreCase) == 0)
                .Value;
            var contentTypeBody = req.Content.Headers.ContentType.MediaType;
            if (req.Content.Headers.ContentType.MediaType != "application/json")
            {
                var bodydata = req.Content.ReadAsStringAsync();
            }
            else
            {
                
            // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                name = name ?? data?.name;
            }
            // Set name to query string or body data
           

            // ~fOR PROCESSING THE dATA PROCESSING LOGIC
            var cts = new CancellationToken();

            if (!string.IsNullOrWhiteSpace(name))
            {
                //Lets Connect DSA ApprenticeshipFeed
                var response =
                    RestCosmosDbHelper.GetSfaPublicVacanySummaryRestHandle(Apprenticeship.AzureFunction.Helper.ConnectionStringConstants.DSAFEEDRESTAPIBASEURI,
                        ConnectionStringConstants.DSAFEEDRESTREQUESTURI, 1, 100);
                var content = response.Content;
                log.Info($"Feed Response :{content}");
                //Lets Connect CosmosDB
                await RestCosmosDbHelper.GetConnectionHandleAsync(ConnectionStringConstants.URICOSMOSDB,
                    ConnectionStringConstants.PRIMARYKEYCOSMOSDB, null, cts);
                await RestCosmosDbHelper.CreateCosmosDatabaseAsync(new Database() { Id = CosmosDbConstants.DatabaseName }, null, cts);
                await RestCosmosDbHelper.CreateCollectionAsync(UriFactory.CreateDatabaseUri(CosmosDbConstants.DatabaseName),
                    new DocumentCollection { Id = CosmosDbConstants.DocumentCollectionName }, null, cts);
                var summaryCollection = RestCosmosDbHelper.GetSerializedResponse(response);
                foreach (var summary in summaryCollection)
                {
                    await RestCosmosDbHelper.CreateDocumentAsync(CosmosDbConstants.DatabaseName,
                        CosmosDbConstants.DocumentCollectionName, summary, null, cts);
                }
                req.CreateResponse(HttpStatusCode.OK, "Successfully executed CosmosDB Execution : " + name);
            }

            //

            return name == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "You successfully executed the function with the parameter : " + name);
        }
    }
}
