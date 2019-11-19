using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Dashboard.Api.Services;
using Dashboard.CloudStorage;
using Dashboard.CloudStorage.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Xunit;

namespace Dashboard.Api.Tests.Services
{
    public class CloudVehicleServiceTests
    {
        [Theory, DefaultAutoData]
        public async Task GetAllAsync_ShouldGetVehicleViewModels(
            List<Vehicle> vehicles,
            [Frozen] ITableHandler<Vehicle> vehicleHandler,
            CloudVehicleService sut)
        {
            vehicleHandler.GetAsync().Returns(vehicles);

            var result = await sut.GetAllAsync();

            result.Should().BeEquivalentTo(vehicles, (x) => x.ExcludingMissingMembers());
        }

        [Theory, DefaultAutoData]
        public async Task GetAllAsync_SingleVehicle_ShouldSetOwner(
            string scheme, 
            HostString host,
            Vehicle vehicle,
            [Frozen] IHttpContextAccessor context,
            [Frozen] ITableHandler<Vehicle> vehicleHandler,
            CloudVehicleService sut)
        {
            var expectedOwner = $"{scheme}://{host.Value}/api/owners/{vehicle.PartitionKey}";
            context.HttpContext.Request.Scheme.Returns(scheme);
            context.HttpContext.Request.Host.Returns(host);
            vehicleHandler.GetAsync().Returns(new List<Vehicle>{ vehicle });

            var result = await sut.GetAllAsync();

            result.First().Owner.Href.Should().Be(expectedOwner);
        }

        [Theory, DefaultAutoData]
        public void GetByOwnerAsync_OwnerIdIsNull_ThrowsArgumentNullException(
            CloudVehicleService sut)
        {
            Func<Task> act = async () => await sut.GetByOwnerAsync(null);

            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("ownerId");
        }

        [Theory, DefaultAutoData]
        public void GetByOwnerAsync_OwnerIdIsEmpty_ThrowsArgumentNullException(
            CloudVehicleService sut)
        {
            Func<Task> act = async () => await sut.GetByOwnerAsync(string.Empty);

            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("ownerId");
        }

        [Theory, DefaultAutoData]
        public async Task GetByOwnerAsync_GivenOwnerId_ShouldGetByPartitionKey(
            string ownerId,
            [Frozen] ITableHandler<Vehicle> vehicleHandler,
            CloudVehicleService sut)
        {
            var results = await sut.GetByOwnerAsync(ownerId);

            await vehicleHandler.Received().GetAsync(ownerId);
        }

        [Theory, DefaultAutoData]
        public async Task GetByOwnerAsync_ShouldGetVehicleViewModels(
            string ownerId,
            List<Vehicle> vehicles,
            [Frozen] ITableHandler<Vehicle> vehicleHandler,
            CloudVehicleService sut)
        {
            vehicleHandler.GetAsync(Arg.Any<string>()).Returns(vehicles);

            var results = await sut.GetByOwnerAsync(ownerId);

            results.Should().BeEquivalentTo(vehicles, (x) => x.ExcludingMissingMembers());
        }

        [Theory, DefaultAutoData]
        public async Task GetByOwnerAsync_SingleVehicle_ShouldSetOwner(
            string ownerId,
            string scheme, 
            HostString host,
            Vehicle vehicle,
            [Frozen] IHttpContextAccessor context,
            [Frozen] ITableHandler<Vehicle> vehicleHandler,
            CloudVehicleService sut)
        {
            var expectedOwner = $"{scheme}://{host.Value}/api/owners/{vehicle.PartitionKey}";
            context.HttpContext.Request.Scheme.Returns(scheme);
            context.HttpContext.Request.Host.Returns(host);
            vehicleHandler.GetAsync(ownerId).Returns(new List<Vehicle>{ vehicle });

            var result = await sut.GetByOwnerAsync(ownerId);

            result.First().Owner.Href.Should().Be(expectedOwner);
        }
    }
}