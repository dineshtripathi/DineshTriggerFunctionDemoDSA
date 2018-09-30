using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs.Host;

namespace Apprenticeship.Function.CosmosDB
{
    public interface IProvideCosmosDbApi
    {
        Task<DocumentClient> ConnectClientAsync(string endpointUrl, string primaryKey, TraceWriter log,CancellationToken cts);
        Task<DocumentClient> ConnectionClientAsync(string connectionString, TraceWriter log,CancellationToken cts);
        Task<ResourceResponse<Database>> CreateDataBaseAsync(Database newDatabase, TraceWriter log, CancellationToken cts);
        Task<ResourceResponse<DocumentCollection>> CreateCollectionAsync(Uri databaseUri, DocumentCollection documentCollection, TraceWriter log, CancellationToken cts);
        Task<ResourceResponse<Document>> CreateDocumentAsync(string databaseName, string collectionName, VacancySummary vacancySummary, TraceWriter log, CancellationToken cts);
        Task<CosmosDbVacancySummary> GetVacancySummaryAsync(string queryOnItem, TraceWriter log, CancellationToken cts);
        Task<IEnumerable<CosmosDbVacancySummary>> GetVacanciesSummaryAsync(string queryItem,TraceWriter log, CancellationToken cts);



    }
}