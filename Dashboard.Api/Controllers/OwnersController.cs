using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Dashboard.Api.Constants;

namespace Dashboard.Api.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Owners)]
    public class OwnersController : ControllerBase
    {
        [HttpGet]
        public Task<IEnumerable<OwnerViewModel>> GetOwners()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id}")]
        public Task<OwnerViewModel> GetOwner(string id)
        {
            throw new NotImplementedException();
        }
    }
}