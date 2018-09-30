﻿using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Apprenticeship.Function.AzureStorage.Queue
{
    public interface IQueueStorageAsync
    {
        Task EnQueueAsync(byte[] content);
        Task<CloudQueueMessage> PeekAsync();
        Task EnQueueAsync(string content);
        Task<CloudQueueMessage> DeQueueAsync();
        Task UpdateQueueMessageAsync(string content);
        Task DeleteMessageAsync(CloudQueueMessage cloudQueueMessage);
        Task DeleteQueueAsync();
        Task ClearQueueAsync();
        int MessageCount();
    }
}
