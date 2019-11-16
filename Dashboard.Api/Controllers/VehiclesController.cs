using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.Api.Services;
using Dashboard.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Dashboard.Api.Constants;

namespace Dashboard.Api.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Vehicles)]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService) => _vehicleService = vehicleService;

        [HttpGet]
        public async Task<IEnumerable<VehicleViewModel>> GetVehicles()
        {
            return await _vehicleService.GetAllAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public Task<VehicleViewModel> GetVehicle(string id)
        {
            throw new NotImplementedException();
        }
    }
}