using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.Models;

namespace backend.Core.IRepositories
{
    public interface IParcelRepository
    {
        Task<IEnumerable<Parcel>> GetParcelsAsync();
        Task<IEnumerable<Parcel>> GetParcelsByWarehouseIdAsync(Guid warehouseId);
        Task<Parcel> GetParcelByIdAsync(Guid id);
        Task AddParcelAsync(Parcel parcel);
        Task UpdateParcelAsync(Parcel parcel);
        Task DeleteParcelAsync(Guid id);
    }
}