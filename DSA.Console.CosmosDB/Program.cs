using System;
using System.Threading;
using Apprenticeship.AzureFunction.Helper;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Apprenticeship.AzureFunction.APIHelper;
using Apprenticeship.Function.AzureStorage.Queue;
using RestSharp;

namespace Apprenticeship.Console.CosmosDB
{
    class Program
    {
        static void  Main(string[] args)
        {
            System.Console.WriteLine("Test -> Lets read the Sitefinity Data first for Gurney Sir");
            var sitefinty=new RestClient("http://local-beta.nationalcareersservice.org.uk");
            var sitefinityrequest=new RestRequest("/api/faa-integration/jobprofilesocs/mapping");
            sitefinityrequest.Method=Method.GET;
            sitefinityrequest.AddJsonBody(new
            {
                name = "Http",
                PageSize = "100",
                FeedSource = "DASFeed"
            });
            sitefinityrequest.JsonSerializer.ContentType = "application/json; charset=utf-8";
            var sitefinityresponse = sitefinty.Execute(sitefinityrequest);
            System.Console.WriteLine($"Response Received from the SiteFinity : {sitefinityresponse.Content} \n");
            System.Console.WriteLine("Test Done");
            System.Console.ReadKey();

            System.Console.WriteLine("1 . Want to parse vacany summary api using Azure Function with QueueTrigger   - Press Q\n");
            System.Console.WriteLine("2 . Want to parse vacany summary api using Azure Function with HttpTrigger    - Press T\n");
            System.Console.WriteLine("3 . Want to parse vacany summary api using Console app without Queue/Http     - Press C\n");
            System.Console.WriteLine("4 . Press X to exit");
            var key=System.Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.T:
                    System.Console.WriteLine(
                        "Functions executed on demand when message is Posted on HTTP  To the HTTPTrigger Function \n");

                    var client =
                        new RestClient(
                            "https://sfadsatriggerfunction.azurewebsites.net/api/DSAAzureTriggerFunction");
                    var request =
                        new RestRequest("");

                    //For local debugging - Enable it
                    //{ 
                 //   client = new RestClient("http://localhost:7071/api/DSAAzureTriggerFunction");
                  //  request = new RestRequest("");
                    //}
               //   request.AddHeader("Authorization", string.Format("Basic {0}", "ZGluZXNoLnRyaXBhdGhpQG9wZW5lbmVyZ2kuY29tOkRpbmVzaEA3Mw=="));
                    request.Method = Method.POST;
                  
                    request.AddJsonBody(new
                    {
                        name = "Http",
                        PageSize = "100",
                        FeedSource = "DASFeed"
                    });
                    request.JsonSerializer.ContentType = "application/json; charset=utf-8";
                   
                    IRestResponse response = client.Execute(request);
                    System.Console.WriteLine("SiteFinity Content {0}");
                    System.Console.WriteLine($"Response Received from the Trigger Function : {response.Content} ,Message :{response.ResponseStatus} ,Error :{response.ErrorMessage} \n");
                    System.Console.WriteLine("Press any key to exit - Function executed and Data inserted into COSMOS DB .Check log and COSMOS DB in the azure");
                    System.Console.ReadKey();
                    break;
                case ConsoleKey.Q:
                    System.Console.WriteLine(
                        "Functions executed on demand when message is written in the  Storage Queue \n");
                    IQueueStorageAsync queueStorage = new QueueStorageAsync(ConnectionStringConstants.STORAGEQUEUENAME, ConnectionStringConstants.STORAGECONNECTIONNAME);
                    queueStorage.EnQueueAsync("ApprenticeshipQueueFunction");
                    System.Console.WriteLine("Press any key to exit - Function executed and Data inserted into COSMOS DB .Check log and COSMOS DB in the azure");
                    System.Console.ReadKey();
                    break;
                case ConsoleKey.C:
                    System.Console.WriteLine("Without Function - Executes the Business Layer API to insert data into CosmosDB");
                    ProcessApiWithoutFunction();
                    System.Console.ReadKey();
                    break;
                case ConsoleKey.X:
                    return;
                default:
                {
                    System.Console.WriteLine("Invalid key entered");
                }
                    break;
            }
           
            // log.Level=TraceLevel.Info;

        }

        private static void ProcessApiWithoutFunction()
        {
            CancellationToken cts = new CancellationToken();
            var response =
                RestCosmosDbHelper.GetSfaPublicVacanySummaryRestHandle(ConnectionStringConstants.DSAFEEDRESTAPIBASEURI,ConnectionStringConstants.DSAFEEDRESTREQUESTURI, 1,
                    100);

            RestCosmosDbHelper.GetConnectionHandleAsync(ConnectionStringConstants.URICOSMOSDB,
                ConnectionStringConstants.PRIMARYKEYCOSMOSDB, null, cts);
            RestCosmosDbHelper.CreateCosmosDatabaseAsync(new Database() {Id = CosmosDbConstants.DatabaseName}, null, cts);
            RestCosmosDbHelper.CreateCollectionAsync(UriFactory.CreateDatabaseUri(CosmosDbConstants.DatabaseName),
                new DocumentCollection {Id = CosmosDbConstants.DocumentCollectionName}, null, cts);
            var summaryCollection = RestCosmosDbHelper.GetSerializedResponse(response);
            foreach (var summary in summaryCollection)
            {
                RestCosmosDbHelper.CreateDocumentAsync(CosmosDbConstants.DatabaseName,
                    CosmosDbConstants.DocumentCollectionName, summary, null, cts);
            }
        }
    }
}
