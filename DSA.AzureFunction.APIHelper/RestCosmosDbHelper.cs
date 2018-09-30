using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using Apprenticeship.Function.CosmosDB;
using Apprenticeship.AzureFunction.Helper;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace Apprenticeship.AzureFunction.APIHelper
{
    public static class RestCosmosDbHelper
    {
        public static IRestResponse GetSfaPublicVacanySummaryRestHandle(string baseUri,string requesturi,int pageNumber,int pageSize)
        {
            return FunctionRestCosmosBusinessLayer.ResVacancySummarytResponse(baseUri,requesturi,pageNumber,pageSize);
        }

        public static IEnumerable<VacancySummary> GetSerializedResponse(IRestResponse response)
        {
         var contents=  JsonConvert.DeserializeObject<CosmosDbVacancySummary>(response.Content);
            foreach (var summary in contents.VacancySummaries)
            {
                yield return summary;
            }
        }
        public static string ParseCollectionAndCreateSingleDocument(
            IRestResponse response, VacancySummary summaryData)
        {
            return JsonConvert.SerializeObject(summaryData);
        }

        public static Task<DocumentClient> GetConnectionHandleAsync(string endpointUrl, string primaryKey,
            TraceWriter log, CancellationToken cts)
        {
            return FunctionRestCosmosBusinessLayer.GetConnectionHandleAsync(endpointUrl, primaryKey, log, cts);
        }

        public static Task<ResourceResponse<Database>> CreateCosmosDatabaseAsync(Database newDatabase, TraceWriter log,
            CancellationToken cts)
        {
            return FunctionRestCosmosBusinessLayer.CreateCosmosDatabaseAsync(newDatabase,  log, cts);
        }

        public static Task<ResourceResponse<DocumentCollection>> CreateCollectionAsync(Uri databaseUri,
            DocumentCollection documentCollection, TraceWriter log, CancellationToken cts)
        {
          //  UriFactory.CreateDatabaseUri(CosmosDbConstants.DatabaseName), new DocumentCollection { Id = CosmosDbConstants.DocumentCollectionName }
            return FunctionRestCosmosBusinessLayer.CreateCollectionAsync(databaseUri, documentCollection, log, cts);
        }

        public static Task<ResourceResponse<Document>> CreateDocumentAsync(string databaseName, string collectionName,
            VacancySummary vacancySummary, TraceWriter log, CancellationToken cts)
        {
            return FunctionRestCosmosBusinessLayer.CreateDocumentAsync(databaseName, collectionName, vacancySummary,
                log, cts);
        }

        public static async Task InsertDocumentIntoCosmosDB(string endpointUri,string key,TraceWriter log, CancellationToken cts,string summaryData)
        {
           var connection= await (FunctionRestCosmosBusinessLayer.GetConnectionHandleAsync(endpointUri, key, null, cts)).ConfigureAwait(false);
            if (connection != null)
            {
              //  connection.CreateDocumentAsync("sfaApreenticeship")
            }
        }
    }
}
