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
    [Route(ApiRoutes.Owners)]
    public class OwnersController : ControllerBase
    {
        private readonly ILogger<VehiclesController> _logger;
        private readonly IVehicleService _vehicleService;

        public OwnersController(ILogger<VehiclesController> logger, 
            IVehicleService vehicleService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public Task<IEnumerable<OwnerViewModel>> GetOwners()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id}")]
        public Task<ActionResult> GetOwner(string id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id}/vehicles")]
        public async Task<ActionResult> GetVehicles(string id)
        {
            IEnumerable<VehicleViewModel> vehicles = null;
            
            try {
                vehicles = await _vehicleService.GetByOwnerAsync(id);
            }
            catch(Exception exception) {
                _logger.LogError(exception, "Error fetching vehicle for owner {ownerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new ErrorInfo(ErrorType.UnhandledError, $"Error fetching vehicles for owner id {id}."));
            }

            return Ok(vehicles);
        }
    }
}