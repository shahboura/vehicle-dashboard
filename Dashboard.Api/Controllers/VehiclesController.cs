using System;
using System.Threading.Tasks;
using Dashboard.Api.Services;
using Dashboard.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using static Dashboard.Api.Constants;

namespace Dashboard.Api.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Vehicles)]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
            => _vehicleService = vehicleService;

        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = await _vehicleService.GetAllAsync();
            return Ok(vehicles);
        }

        [HttpGet]
        [Route("{id}")]
        public Task<VehicleViewModel> GetVehicle(string id)
        {
            throw new NotImplementedException();
        }
    }
}