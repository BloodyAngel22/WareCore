using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.IRepositories;
using backend.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories
{
    public class CategoryRepository(AppDbContext context) : ICategoryRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id) ?? throw new Exception("Category not found");

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<int>> GetAvailableCategoryIdsAsync()
        {
            return await _context.Categories.Select(x => x.Id).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();
            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Category not found");

            return category;
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var categoryToUpdate = await _context.Categories.FindAsync(category.Id) ?? throw new Exception("Category not found");

            categoryToUpdate.Name = category.Name;

            await _context.SaveChangesAsync();
        }
    }
}