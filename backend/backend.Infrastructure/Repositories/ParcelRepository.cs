using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.IRepositories;
using backend.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories
{
    public class ParcelRepository(AppDbContext context) : IParcelRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddParcelAsync(Parcel parcel)
        {
            await _context.Parcels.AddAsync(parcel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteParcelAsync(Guid id)
        {
            var parcel = await _context.Parcels.FindAsync(id) ?? throw new Exception("Parcel not found");

            _context.Parcels.Remove(parcel);

            await _context.SaveChangesAsync();
        }

        public async Task<Parcel> GetParcelByIdAsync(Guid id)
        {
            var parcel = await _context.Parcels.AsNoTracking().Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Parcel not found");

            return parcel;
        }

        public async Task<IEnumerable<Parcel>> GetParcelsAsync()
        {
            var parcels = await _context.Parcels.AsNoTracking().Include(x => x.Category).ToListAsync();

            return parcels;
        }

        public async Task<IEnumerable<Parcel>> GetParcelsByWarehouseIdAsync(Guid warehouseId)
        {
            var warehouse = await _context.Warehouses.AsNoTracking().Include(x => x.Shelves).ThenInclude(x => x.Parcels).ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Id == warehouseId) ?? throw new Exception("Warehouse not found");

            return warehouse.Shelves.SelectMany(x => x.Parcels);
        }

        public async Task UpdateParcelAsync(Parcel parcel)
        {
            var parcelToUpdate = await _context.Parcels.FindAsync(parcel.Id) ?? throw new Exception("Parcel not found");

            parcelToUpdate.Name = parcel.Name;
            parcelToUpdate.Weight = parcel.Weight;
            parcelToUpdate.ShelfId = parcel.ShelfId;
            parcelToUpdate.CategoryId = parcel.CategoryId;

            await _context.SaveChangesAsync();
        }
    }
}