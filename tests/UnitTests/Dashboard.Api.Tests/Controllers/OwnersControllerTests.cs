using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Dashboard.Api.Services;
using Dashboard.Api.Tests;
using Dashboard.Api.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Dashboard.Api.Tests.Controllers
{
    public class OwnersControllerTests
    {
        [Theory, DefaultAutoData]
        public async Task GetVehicles_OwnerIdNull_Returns400(
            OwnersController sut)
        {
            var result = (await sut.GetVehicles(null)) as BadRequestObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            var errorInfo = result.Value as ErrorInfo;
            errorInfo.Should().NotBeNull();
            errorInfo.Type.Should().Be(ErrorType.Validation);
            errorInfo.Messages.Should().BeEquivalentTo($"OwnerId is required.");
        }

        [Theory, DefaultAutoData]
        public async Task GetVehicles_NoVehiclesFound_Returns404(
            string ownerId,
            [Frozen] IVehicleService vehicleService,
            OwnersController sut)
        {
            vehicleService.GetByOwnerAsync(Arg.Any<string>())
                .Returns((IEnumerable<VehicleViewModel>) null);

            var result = (await sut.GetVehicles(ownerId)) as NotFoundObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            var errorInfo = result.Value as ErrorInfo;
            errorInfo.Should().NotBeNull();
            errorInfo.Type.Should().Be(ErrorType.InvalidOperation);
            errorInfo.Messages.Should().BeEquivalentTo($"No vehicles found for ownerId {ownerId}.");
        }

        [Theory, DefaultAutoData]
        public async Task GetVehicles_ExceptionThrown_Returns500(
            string ownerId,
            [Frozen] IVehicleService vehicleService,
            OwnersController sut)
        {
            vehicleService.GetByOwnerAsync(Arg.Any<string>()).Throws(new Exception());

            var result = (await sut.GetVehicles(ownerId)) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            var errorInfo = result.Value as ErrorInfo;
            errorInfo.Should().NotBeNull();
            errorInfo.Type.Should().Be(ErrorType.UnhandledError);
            errorInfo.Messages.Should().BeEquivalentTo($"Error fetching vehicles for owner id {ownerId}.");
        } 

        [Theory, DefaultAutoData]
        public async Task GetVehicles_ExceptionThrown_ShouldLog(
            string ownerId,
            Exception exception,
            [Frozen] ILogger<OwnersController> logger,
            [Frozen] IVehicleService vehicleService,
            OwnersController sut)
        {
            vehicleService.GetByOwnerAsync(Arg.Any<string>()).Throws(exception);

            await sut.GetVehicles(ownerId);

            logger.ReceivedWithAnyArgs().LogError(exception, $"Error fetching vehicle for owner {ownerId}.");
        }

        [Theory, DefaultAutoData]
        public async Task GetVehicles_GivenOwnerId_FetchByPassedOwnerId(
            string ownerId,
            [Frozen] IVehicleService vehicleService,
            OwnersController sut)
        {
            await sut.GetVehicles(ownerId);

            await vehicleService.Received().GetByOwnerAsync(ownerId);
        }

        [Theory, DefaultAutoData]
        public async Task GetVehicles_FetchedListOfVehicles_Returns200WithVehicles(
            string ownerId,
            IEnumerable<VehicleViewModel> vehicles,
            [Frozen] IVehicleService vehicleService,
            OwnersController sut)
        {
            vehicleService.GetByOwnerAsync(Arg.Any<string>()).Returns(vehicles);

            var result = (await sut.GetVehicles(ownerId)) as OkObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            var vehiclesResult = result.Value as IEnumerable<VehicleViewModel>;
            vehiclesResult.Should().NotBeNull();
            vehiclesResult.Should().BeEquivalentTo(vehicles);
        }
    }
}