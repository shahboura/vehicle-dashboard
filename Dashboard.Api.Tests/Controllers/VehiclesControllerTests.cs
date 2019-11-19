using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Dashboard.Api.Controllers;
using Dashboard.Api.Services;
using Dashboard.Api.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Dashboard.Api.Tests.Controllers
{
    public class VehiclesControllerTests
    {
        [Theory, DefaultAutoData]
        public async Task GetVehicles_ExceptionThrown_Returns500(
            [Frozen] IVehicleService vehicleService,
            VehiclesController sut)
        {
            vehicleService.GetAllAsync().Throws(new Exception());

            var result = (await sut.GetVehicles()) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            var errorInfo = result.Value as ErrorInfo;
            errorInfo.Should().NotBeNull();
            errorInfo.Type.Should().Be(ErrorType.UnhandledError);
            errorInfo.Messages.Should().BeEquivalentTo("Error fetching vehicles from storage.");
        }

        [Theory, DefaultAutoData]
        public async Task GetVehicles_FetchedListOfVehicles_Returns200WithVehicles(
            IEnumerable<VehicleViewModel> vehicles,
            [Frozen] IVehicleService vehicleService,
            VehiclesController sut)
        {
            vehicleService.GetAllAsync().Returns(vehicles);

            var result = (await sut.GetVehicles()) as OkObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            var vehiclesResult = result.Value as IEnumerable<VehicleViewModel>;
            vehiclesResult.Should().NotBeNull();
            vehiclesResult.Should().BeEquivalentTo(vehicles);
        }
    }
}