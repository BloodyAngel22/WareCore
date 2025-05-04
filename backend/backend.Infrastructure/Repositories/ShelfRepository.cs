using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.Exceptions;
using backend.Core.IRepositories;
using backend.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories
{
    public class ShelfRepository(AppDbContext context) : IShelfRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<bool> CanUpdateShelfCategoryAsync(Guid id, int newCategoryId)
        {
            var shelf = await GetShelfByIdAsync(id);

            return await _context.Shelves.AnyAsync(x => x.Id == id && x.Parcels.Count() != 0 && x.CategoryId == newCategoryId || x.Parcels.Count() == 0);
        }

        public async Task<bool> CanUpdateShelfWeightAsync(Guid id, float newWeight)
        {
            return await _context.Shelves.AnyAsync(x => x.Id == id && x.Parcels.Sum(x => x.Weight) <= newWeight);
        }

        public async Task CreateShelfAsync(Shelf shelf)
        {
            await _context.Shelves.AddAsync(shelf);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteShelfAsync(Guid id)
        {
            var shelf = await _context.Shelves.FindAsync(id) ?? throw new Exception("Shelf not found");

            _context.Shelves.Remove(shelf);

            await _context.SaveChangesAsync();
        }

        public async Task<Shelf> GetAvailableShelfWithCategoryAndForWeight(Guid warehouseId, int categoryId, float weight)
        {
            return await _context.Shelves.Where(shelf => 
                shelf.WarehouseId == warehouseId &&
                shelf.CategoryId == categoryId
            ).Select(shelf => new
            {
                Shelf = shelf,
                CurrentWeight = shelf.Parcels.Sum(parcel => parcel.Weight)
            }).Where(x => x.Shelf.ShelfWeight >= x.CurrentWeight + weight).Select(x => x.Shelf).FirstOrDefaultAsync() ?? throw new CustomException("Shelf not found or overweight");
        }

        public async Task<Shelf> GetShelfByIdAsync(Guid id)
        {
            var shelf = await _context.Shelves.AsNoTracking().Include(x => x.Category).Include(x => x.Parcels).ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Id == id) ?? throw new CustomException("Shelf not found");

            return shelf;
        }

        public async Task<IEnumerable<Shelf>> GetShelvesAsync()
        {
            var shelves = await _context.Shelves.AsNoTracking().Include(x => x.Category).Include(x => x.Parcels).ThenInclude(x => x.Category).ToListAsync();

            return shelves;
        }

        public async Task<IEnumerable<Shelf>> GetShelvesByWarehouseId(Guid warehouseId)
        {
            var warehouse = await _context.Warehouses.AsNoTracking().Include(x => x.Shelves).ThenInclude(x => x.Category).Include(x => x.Shelves).ThenInclude(x => x.Parcels).ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Id == warehouseId) ?? throw new Exception("Warehouse not found");

            return warehouse.Shelves;
        }

        public async Task UpdateShelfAsync(Shelf shelf)
        {
            var shelfToUpdate = await _context.Shelves.FindAsync(shelf.Id) ?? throw new Exception("Shelf not found");

            shelfToUpdate.ShelfWeight = shelf.ShelfWeight;

            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == shelf.CategoryId) ?? throw new Exception("Category not found");

            shelfToUpdate.Category = category;

            await _context.SaveChangesAsync();
        }
    }
}