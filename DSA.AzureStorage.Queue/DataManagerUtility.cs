using System;
using Microsoft.WindowsAzure.Storage;

namespace Apprenticeship.Function.AzureStorage.Queue
{
    /// <summary>
    /// A helper class to support common Azure Data Manager operations
    /// </summary>
    public class DataManagerUtility
    {
        private static CloudStorageAccount storageAccount;

        private static CloudStorageAccount GetStorageAccount(string connectionString)
        {
            return storageAccount ?? (storageAccount = CloudStorageAccount.Parse(connectionString));
        }
    }
}
