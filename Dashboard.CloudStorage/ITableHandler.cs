using System.Collections.Generic;
using System.Threading.Tasks;
using Dashboard.CloudStorage.Entities;
using Microsoft.Azure.Cosmos.Table;

namespace Dashboard.CloudStorage
{
    public interface ITableHandler<T>
        where T : class, ITableEntity, IEntity, new()
    {        
        Task<IEnumerable<T>> GetAsync();
        
        Task<IEnumerable<T>> GetAsync(string partitionKey);

        Task<T> GetAsync(string partitionKey, string rowKey);

        Task<T> InsertOrReplaceAsync(T entity);
    }
}