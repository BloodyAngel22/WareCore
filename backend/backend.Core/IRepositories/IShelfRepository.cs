using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.Models;

namespace backend.Core.IRepositories
{
    public interface IShelfRepository
    {
        Task<IEnumerable<Shelf>> GetShelvesAsync();
        Task<IEnumerable<Shelf>> GetShelvesByWarehouseId(Guid warehouseId);
        Task<Shelf> GetShelfByIdAsync(Guid id);
        Task CreateShelfAsync(Shelf shelf);
        Task UpdateShelfAsync(Shelf shelf);
        Task DeleteShelfAsync(Guid id);

        Task<Shelf> GetAvailableShelfWithCategoryAndForWeight(Guid warehouseId, int categoryId, float weight);
        Task<bool> CanUpdateShelfWeightAsync(Guid id, float newWeight);
        Task<bool> CanUpdateShelfCategoryAsync(Guid id, int newCategoryId);
    }
}