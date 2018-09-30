using System;
using System.Threading;
using System.Threading.Tasks;
using Apprenticeship.Dummy.Database;
using Apprenticeship.Function.CosmosDB;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs.Host;
using RestSharp;

namespace Apprenticeship.AzureFunction.Helper
{
    public static class FunctionRestCosmosBusinessLayer
    {
        //Note it will not be static connection. The Connection will be per request.
        //used static just for demo sample 
        public static IProvideCosmosDbApi CosmosDbApi { get; set; }
        public static Task<DocumentClient> GetConnectionHandleAsync(string endpointUrl,string primaryKey,TraceWriter log,CancellationToken cts)
        {
            CosmosDbApi = new CosmosDbApi();
            var document = CosmosDbApi.ConnectClientAsync(endpointUrl,primaryKey,log,cts);
            return  document;
        }

        public static Task<ResourceResponse<Database>> CreateCosmosDatabaseAsync(Database newDatabase, TraceWriter log, CancellationToken cts)
        {
            return CosmosDbApi.CreateDataBaseAsync(newDatabase, log, cts);
        }

        public static Task<ResourceResponse<DocumentCollection>> CreateCollectionAsync(Uri databaseUri,
            DocumentCollection documentCollection, TraceWriter log, CancellationToken cts)
        {
            return CosmosDbApi.CreateCollectionAsync(databaseUri, documentCollection, log, cts);
        }

        public static Task<ResourceResponse<Document>> CreateDocumentAsync(string databaseName, string collectionName, VacancySummary vacancySummary, TraceWriter log, CancellationToken cts)
        {
            return CosmosDbApi.CreateDocumentAsync(databaseName, collectionName, vacancySummary, log, cts);
        }
        public static void FeedDummyDataIntoEf(TraceWriter log)
        {
            dummyDataEntities data = new dummyDataEntities();
            UserFeedData userfeed = new UserFeedData
            {
                KeyId = new Guid(),
                FirstName = "ApprenticeShip",
                LastName = "Feed"
            };
            log.Info($"FeedModelValue : {userfeed.FirstName}, {userfeed.LastName}");
            data.UserFeedDatas.Add(userfeed);
            data.SaveChanges();
        }

        public static IRestResponse ResVacancySummarytResponse(string baseUri,string requesturi,object pageNumber,object pageSize)
        { // "https://pre-restapi.findapprenticeship.service.gov.uk"
            var client = new RestClient(baseUri);
            var request = new RestRequest(requesturi);
            request.AddParameter("Page", pageNumber);
            request.AddParameter("PageSize", pageSize);
            IRestResponse response = client.Execute(request);
            return response;
        }
    }
}
