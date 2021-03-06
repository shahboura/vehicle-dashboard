using System.Collections.Generic;
using System.Threading.Tasks;
using Dashboard.CloudStorage.Entities;
using Dashboard.CloudStorage.Extensions;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace Dashboard.CloudStorage
{
    public class TableHandler<T> : ITableHandler<T>
        where T : class, ITableEntity, IEntity, new()
    {
        private readonly CloudTable _table;

        public TableHandler(IConfiguration config)
        {
            if (config is null)
            {
                throw new System.ArgumentNullException(nameof(config));
            }

            var entity = new T();
            var connectionString = config["AzureCloudStorage"];
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var _client = storageAccount.CreateCloudTableClient();
            _table = _client.GetTableReference(entity.TableName);
        }

        public Task CreateIfNotExistsAsync()
        {
            return _table.CreateIfNotExistsAsync();
        }

        public async Task<T> GetAsync(string partitionKey, string rowKey)
        {
            partitionKey.ThrowIfNullOrWhiteSpace(nameof(partitionKey));
            rowKey.ThrowIfNullOrWhiteSpace(nameof(rowKey));

            var retrieve = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var result = await _table.ExecuteAsync(retrieve);
            return result.Result as T;
        }

        public async Task<List<T>> GetAsync()
        {
            return await ExecuteQueryAsync(new TableQuery<T>());
        }

        public async Task<List<T>> GetAsync(string partitionKey)
        {
            partitionKey.ThrowIfNullOrWhiteSpace(nameof(partitionKey));

            var query = new TableQuery<T>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey",
                    QueryComparisons.Equal, partitionKey));

            return await ExecuteQueryAsync(query);
        }        

        public async Task<T> InsertOrReplaceAsync(T entity)
        {
            entity.ThrowIfNull(nameof(entity));

            var insert = TableOperation.InsertOrReplace(entity);
            var result = await _table.ExecuteAsync(insert);
            return result.Result as T;
        }

        private async Task<List<T>> ExecuteQueryAsync(TableQuery<T> query)
        {
            var results = new List<T>();
            TableContinuationToken token = null;

            do
            {
                var resultSegment = await _table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                results.AddRange(resultSegment.Results);
            } while (token != null);

            return results;
        }
    }
}