using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ILogger<OwnersController> _logger;
        private readonly IVehicleService _vehicleService;

        public OwnersController(ILogger<OwnersController> logger, 
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
        [ProducesResponseType(typeof(VehicleViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorInfo), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorInfo), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorInfo), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetVehicles(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new ErrorInfo(ErrorType.Validation, "OwnerId is required."));
            }

            IEnumerable<VehicleViewModel> vehicles = null;
            
            try {
                vehicles = await _vehicleService.GetByOwnerAsync(id);
            }
            catch(Exception exception) {
                _logger.LogError(exception, "Error fetching vehicle for owner {ownerId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new ErrorInfo(ErrorType.UnhandledError, $"Error fetching vehicles for owner id {id}."));
            }

            if(vehicles == null || !vehicles.Any())
            {
                return NotFound(new ErrorInfo(ErrorType.InvalidOperation, $"No vehicles found for ownerId {id}."));
            }

            return Ok(vehicles);
        }
    }
}