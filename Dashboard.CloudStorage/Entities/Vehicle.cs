using Microsoft.Azure.Cosmos.Table;

namespace Dashboard.CloudStorage.Entities
{
    public class Vehicle : TableEntity, IEntity
    {        
        public string TableName => "vehicleinfo";
        public string RegNumber { get; set; }
        public bool Connected { get; set; }
    }
}