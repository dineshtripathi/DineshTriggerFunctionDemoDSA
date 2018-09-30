using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Json;

namespace Apprenticeship.Function.CosmosDB
{
    public class CosmosDbApi:IProvideCosmosDbApi
    {
        // ReSharper disable once InconsistentNaming
        private DocumentClient client;

        private ResourceResponse<Database> databaseConnected;
        public async Task<DocumentClient> ConnectClientAsync(string endpointUrl, string primaryKey,TraceWriter log,CancellationToken cts)
        {
            try
            {
                client=  new DocumentClient(new Uri(endpointUrl), primaryKey);
                log?.Info($"COSMOS DB Endpoint Connected");
                return await Task.FromResult(client).ConfigureAwait(false);
            }
            catch (DocumentClientException de)
            {
                var baseException = de.GetBaseException();
                log?.Info($"{de.StatusCode} error occurred: {de.Message}, Message: {baseException.Message}");
            }
            catch (Exception e)
            {
                var baseException = e.GetBaseException();
                log?.Info($"error occurred: {e.Message}, Message: {baseException.Message}");
            }
            finally
            {
                log?.Info($"Cleared Finally");
            }
            return null;
        }

        public async Task<DocumentClient> ConnectionClientAsync(string connectionString, TraceWriter log, CancellationToken cts)
        {
            await Task.FromResult(0);
            throw new NotImplementedException();
        }

        public async Task<ResourceResponse<Database>> CreateDataBaseAsync(Database newDatabase, TraceWriter log, CancellationToken cts)
        {
            if (client != null && client.WriteEndpoint != null)
            {
                try
                {
                    log?.Info($" Creating Database");
                    databaseConnected = await client.CreateDatabaseIfNotExistsAsync(newDatabase).ConfigureAwait(false);
                    log?.Info($" Database Created");
                    return databaseConnected;
                }
                catch (Exception e)
                {
                    log?.Info($" Error while creating database : {e}");
                    throw;
                }
              
            }
            return null;

        }

        public async Task<ResourceResponse<DocumentCollection>> CreateCollectionAsync(Uri databaseUri, DocumentCollection documentCollection, TraceWriter log, CancellationToken cts)
        {
            if (client != null && client.WriteEndpoint != null)
            {
              //  if (databaseConnected != null)
                try
                {
                    log?.Info($" Creating Collection");
                    ResourceResponse<DocumentCollection> documentResponse =
                        await client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, documentCollection)
                            .ConfigureAwait(false);
                    log?.Info($" Collection created");
                    return documentResponse;
                }
                catch (Exception e)
                {
                    log?.Info($" Error while creating database : {e}");
                    throw;
                }
            }
            return null;
        }

        public async Task<ResourceResponse<Document>> CreateDocumentAsync(string databaseName, string collectionName, VacancySummary vacancySummary, TraceWriter log, CancellationToken cts)
        {
            try
            {
               
               //await ConnectClientAsync("https://sfacosmosdb.documents.azure.com:443/",
               //     "yeFYPdLaiTBP7cxEVxBB8x8gaJM5H5DCcthqZuVBUWAzwu5IH6UEpDMTBxFxPqjhArIRjAPtHDKsD1MJbSY2ZA ==", null,
               //     new CancellationToken());
               Thread.Sleep(500);
                //ResourceResponse<Document> writeResponse = await this.client.CreateDocumentAsync(
                //    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), vacancySummary).ConfigureAwait(false);
                //return writeResponse;

                log?.Info($" inserting new record ");
                ResourceResponse<Document> writeResponse = await client.CreateDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), vacancySummary).ConfigureAwait(false);
                log?.Info($" inserted new record ");

                return writeResponse;


                // READ BELOW COMMENTED FOR TESTING

                //log?.Info($" check if the record exist  with GUid: {vacancySummary.VacancyGuid}");
                //ResourceResponse<Document> readResponse = await this.client.ReadDocumentAsync(
                //    UriFactory.CreateDocumentUri(databaseName, collectionName, vacancySummary.VacancyGuid)).ConfigureAwait(false);
                //log?.Info($" Found record exist  with GUid: {vacancySummary.VacancyGuid}");
                //return readResponse;


            }
            catch (DocumentClientException de)
            {
                log?.Info($" IF record not found then got Document client exception : {de.Message}");
                if (de.StatusCode == HttpStatusCode.NotFound || de.StatusCode==HttpStatusCode.NoContent)
                {
                    log?.Info($" inserting new record after Document client exception : {de.Message}");
                    ResourceResponse<Document> writeResponse = await client.CreateDocumentAsync(
                        UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), vacancySummary).ConfigureAwait(false);
                    log?.Info($" inserted new record ");

                    return writeResponse;
                }
                
            }
            catch (Exception exception)
            {
                log?.Info($" Application exception occoured : {exception.Message}");
            }
            return null;
        }

        public async Task<CosmosDbVacancySummary> GetVacancySummaryAsync(string queryOnItem, TraceWriter log, CancellationToken cts)
        {
            await Task.FromResult(0);
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CosmosDbVacancySummary>> GetVacanciesSummaryAsync(string queryItem, TraceWriter log, CancellationToken cts)
        {
            await Task.FromResult(0);
            throw new NotImplementedException();
        }
        private void WriteToConsoleAndPromptToContinue(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }
    }
}
