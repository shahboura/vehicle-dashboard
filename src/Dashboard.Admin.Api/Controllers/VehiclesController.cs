using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dashboard.CloudStorage;
using Dashboard.CloudStorage.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly Random _random;
        private readonly ITableHandler<Vehicle> _vehicleClient;

        public VehiclesController(Random random,
            ITableHandler<Vehicle> vehicleClient)
        {
            _random = random;
            _vehicleClient = vehicleClient;
        }

        [HttpPost]
        [Route("ping")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Ping()
        {
            var tasks = new List<Task>();
            var vehicles = await _vehicleClient.GetAsync();

            vehicles.ForEach((v) => {
                v.Connected = _random.Next(0, 2) == 1;
                tasks.Add(_vehicleClient.InsertOrReplaceAsync(v));
            });

            await Task.WhenAll(tasks);
            return Ok();
        } 
    }
}