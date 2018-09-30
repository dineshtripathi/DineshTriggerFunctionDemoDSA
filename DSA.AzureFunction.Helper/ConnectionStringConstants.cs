using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Apprenticeship.AzureFunction.Helper
{
    public static class ConnectionStringConstants
    {
        public static string DSAFEEDRESTAPIBASEURI = "https://pre-restapi.findapprenticeship.service.gov.uk";
        public static string DSAFEEDRESTREQUESTURI = "public/vacancysummaries";
        public static string URICOSMOSDB = "https://sfacosmosdb.documents.azure.com:443/";
        public static string PRIMARYKEYCOSMOSDB = "ephnRCb49u3xYFRI80xa5eIoU3IX25XXNaoQhrMi4u1X6w1xYolFTqpebgIpYm80Kh7h6PMTi3NPAQhosj7smA==";
        public static string STORAGEACCOUNTNAME = "sfabetastorage";
        public static string STORAGEKEYNAME = "ljxSmczE4I1hGduI1Ge8bS7u7wzfOex2kgs6btpCXTNKufp1CYz/QEVlGx0SaqUR1CegamUVEy8UScSxUWeUGQ==";
        public static string STORAGECONNECTIONNAME = "DefaultEndpointsProtocol=https;AccountName=sfabetastorage;AccountKey=ljxSmczE4I1hGduI1Ge8bS7u7wzfOex2kgs6btpCXTNKufp1CYz/QEVlGx0SaqUR1CegamUVEy8UScSxUWeUGQ==;EndpointSuffix=core.windows.net";
        public static string STORAGEQUEUENAME = "vacancysummaryqueueitem";
        public static string STORAGEQUEUEENDPOINT = "https://sfabetastorage.queue.core.windows.net/";
        public static string STORAGEQUEUEURL = "https://sfabetastorage.queue.core.windows.net/vacancysummaryqueueitem";
    }
}
