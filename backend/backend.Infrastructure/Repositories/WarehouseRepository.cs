using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.IRepositories;
using backend.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories
{
    public class WarehouseRepository(AppDbContext context) : IWarehouseRepository
    {
        private readonly AppDbContext _context = context;

        public async Task CreateWarehouseAsync(Warehouse warehouse)
        {
            await _context.Warehouses.AddAsync(warehouse);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWarehouseAsync(Guid id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id) ?? throw new Exception("Warehouse not found");
            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Guid>> GetAvailableWarehouseIdsAsync()
        {
            return await _context.Warehouses.Select(x => x.Id).ToListAsync();
        }

        public async Task<Warehouse> GetWarehouseByIdAsync(Guid id)
        {
            var warehouse = await _context.Warehouses.AsNoTracking().Include(x => x.Shelves).ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Warehouse not found");
            return warehouse;
        }

        public async Task<IEnumerable<Warehouse>> GetWarehousesAsync()
        {
            var warehouses = await _context.Warehouses.AsNoTracking().Include(x => x.Shelves).ThenInclude(x => x.Category).ToListAsync();
            return warehouses;
        }

        public async Task<bool> HasWarehouseWithId(Guid id)
        {
            var hasWarehouse = await _context.Warehouses.AnyAsync(x => x.Id == id);

            return hasWarehouse;
        }

        public async Task UpdateWarehouseAsync(Warehouse warehouse)
        {
            var warehouseToUpdate = await _context.Warehouses.FindAsync(warehouse.Id) ?? throw new Exception("Warehouse not found");

            warehouseToUpdate.Name = warehouse.Name;

            await _context.SaveChangesAsync();
        }
    }
}