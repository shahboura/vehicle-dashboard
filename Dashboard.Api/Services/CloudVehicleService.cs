using System.Collections.Generic;
using System.Threading.Tasks;
using Dashboard.CloudStorage.Entities;
using Dashboard.CloudStorage.Extensions;
using Dashboard.CloudStorage;
using Dashboard.Api.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using static Dashboard.Api.Constants;

namespace Dashboard.Api.Services
{
    public class CloudVehicleService : IVehicleService
    {
        private readonly string _host;
        private readonly ITableHandler<Vehicle> _vehicleClient;

        public CloudVehicleService(IHttpContextAccessor httpContextAccessor,
            ITableHandler<Vehicle> vehicleClient)
        {
            if (httpContextAccessor is null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            var request = httpContextAccessor.HttpContext.Request;
            _host = $"{request.Scheme}://{request.Host.Value}";
            _vehicleClient = vehicleClient ?? throw new ArgumentNullException(nameof(vehicleClient));
        }

        public async Task<IEnumerable<VehicleViewModel>> GetAllAsync()
        {
            var vehicles = await _vehicleClient.GetAsync();
            return BuildViewModel(vehicles);
        }

        public async Task<IEnumerable<VehicleViewModel>> GetByOwnerAsync(string ownerId)
        {
            ownerId.ThrowIfNullOrWhiteSpace(nameof(ownerId));

            var vehicles = await _vehicleClient.GetAsync(ownerId);
            return BuildViewModel(vehicles);
        }

        private IEnumerable<VehicleViewModel> BuildViewModel(IEnumerable<Vehicle> vehicles)
        {
            var result = new List<VehicleViewModel>();
            foreach (var vehicle in vehicles)
            {
                result.Add(new VehicleViewModel(
                    vehicle.RegNumber,
                    vehicle.Connected,
                    $"{_host}/{ApiRoutes.Owners}/{vehicle.PartitionKey}"
                ));
            }

            return new List<VehicleViewModel>(result);
        }
    }
}