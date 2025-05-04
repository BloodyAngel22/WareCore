using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.Models;

namespace backend.Core.IRepositories
{
    public interface IWarehouseRepository
    {
        Task<IEnumerable<Warehouse>> GetWarehousesAsync();
        Task<Warehouse> GetWarehouseByIdAsync(Guid id);
        Task CreateWarehouseAsync(Warehouse warehouse);
        Task UpdateWarehouseAsync(Warehouse warehouse);
        Task DeleteWarehouseAsync(Guid id);

        Task<bool> HasWarehouseWithId(Guid id);
        Task<IEnumerable<Guid>> GetAvailableWarehouseIdsAsync();
    }
}