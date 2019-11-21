using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dashboard.CloudStorage;
using Dashboard.CloudStorage.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;

namespace Dashboard.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly Random _random;
        private readonly ITableHandler<Vehicle> _vehicleClient;
        private readonly ITableHandler<Owner> _ownerClient;

        public SeedController(Random random,
            ITableHandler<Vehicle> vehicleClient,
            ITableHandler<Owner> ownerClient)
        {
            _random = random;
            _vehicleClient = vehicleClient;
            _ownerClient = ownerClient;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Seed()
        {
            var tasks = new List<Task>();

            tasks.Add(_ownerClient.CreateIfNotExistsAsync());
            tasks.Add(_vehicleClient.CreateIfNotExistsAsync());

            await Task.WhenAll(tasks);
            tasks.Clear();

            var owner1 = GenerateOwner("Kalles Grustransporter AB", "Cementvägen 8, 111 11 Södertälje");
            tasks.Add(_ownerClient.InsertOrReplaceAsync(owner1));
            tasks.AddRange(AddVehicles(owner1, "ABC123", "DEF456", "GHI789"));

            var owner2 = GenerateOwner("Johans Bulk AB", "Balkvägen 12, 222 22 Stockholm");
            tasks.Add(_ownerClient.InsertOrReplaceAsync(owner2));
            tasks.AddRange(AddVehicles(owner2, "JKL012", "MNO345"));

            var owner3 = GenerateOwner("Haralds Värdetransporter AB", "Budgetvägen 1, 333 33 Uppsala");
            tasks.Add(_ownerClient.InsertOrReplaceAsync(owner3));
            tasks.AddRange(AddVehicles(owner3, "PQR678", "STU901"));

            await Task.WhenAll(tasks);

            /* TODO: we need to refactor this, it's place is in respective controller 
            return CREATED 201 instead with resourceUri
            i.e. VehiclesController or OwnersController */
            return Ok();
        }

        private static Task<T> InsertEntity<T>(ITableHandler<T> handler, Func<T> generator)
            where T : class, ITableEntity, IEntity,new()
        {
            var entity = generator();
            return handler.InsertOrReplaceAsync(entity);
        }

        private IList<Task<Vehicle>> AddVehicles(Owner owner, params string[] regNumbers)
        {
            var tasks = new List<Task<Vehicle>>();

            foreach(var regNumber in regNumbers)
            {
                tasks.Add(_vehicleClient.InsertOrReplaceAsync(GenerateVehicle(owner, regNumber)));
            }

            return tasks;
        }

        private Vehicle GenerateVehicle(Owner owner, string regNumber)
        {
            var guid = Guid.NewGuid();
            return new Vehicle
            {
                PartitionKey = owner.RowKey,
                RowKey = guid.ToString(),
                RegNumber = regNumber,
                Connected = _random.Next(0, 2) == 1
            };
        }

        private Owner GenerateOwner(string name, string address)
        {
            var guid = Guid.NewGuid().ToString();
            return new Owner
            {
                PartitionKey = guid,
                RowKey = guid,
                Name = name,
                Address = address
            };
        }
    }
}