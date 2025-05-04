using Microsoft.EntityFrameworkCore;
using backend.Core.Models;

namespace backend.Infrastructure
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1, Name = "Electronics" },
				new Category { Id = 2, Name = "Clothing" },
				new Category { Id = 3, Name = "Books" }
			);
        }
		
		public DbSet<Warehouse> Warehouses { get; set; }
		public DbSet<Shelf> Shelves { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Parcel> Parcels { get; set; }
    }
}