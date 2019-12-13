using Microsoft.Azure.Cosmos.Table;

namespace Dashboard.CloudStorage.Entities
{
    public class Owner : TableEntity, IEntity
    {
        public string TableName => "ownerinfo";
        public string Name { get; set; }
        public string Address { get; set; }
    }
}