using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dashboard.Api.Services;
using Dashboard.Api.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Dashboard.Api.Constants;

namespace Dashboard.Api.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Vehicles)]
    public class VehiclesController : ControllerBase
    {
        private readonly ILogger<VehiclesController> _logger;
        private readonly IVehicleService _vehicleService;

        public VehiclesController(ILogger<VehiclesController> logger, 
            IVehicleService vehicleService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(VehicleViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorInfo), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVehicles()
        {
            IEnumerable<VehicleViewModel> vehicles = null;
            
            try {
                vehicles = await _vehicleService.GetAllAsync();
            }
            catch(Exception exception) {
                _logger.LogError(exception.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new ErrorInfo(ErrorType.UnhandledError, "Error fetching vehicles from storage."));
            }

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