using System.Collections.Generic;
using System.Threading.Tasks;
using Dashboard.Api.ViewModels;

namespace Dashboard.Api.Services
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleViewModel>> GetAllAsync();

        Task<IEnumerable<VehicleViewModel>> GetByOwnerAsync(string ownerId);
    }
}