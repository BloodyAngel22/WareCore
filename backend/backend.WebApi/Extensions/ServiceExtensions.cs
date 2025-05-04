using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Application.DTOs.Request;
using backend.Application.Services;
using backend.Application.Validators;
using backend.Core.IRepositories;
using backend.Infrastructure;
using backend.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace backend.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IShelfRepository, ShelfRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IParcelRepository, ParcelRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<WarehouseService>();
            services.AddScoped<ShelfService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<ParcelService>();

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<WarehouseDTORequest>, WarehouseDTORequestValidator>();
            services.AddScoped<IValidator<ShelfDTORequest>, ShelfDTORequestValidator>();
            services.AddScoped<IValidator<CategoryDTORequest>, CategoryDTORequestValidator>();
            services.AddScoped<IValidator<ParcelDTORequest>, ParcelDTORequestValidator>();

            return services;
        }

        public static IServiceCollection AddPostgresDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));

            return services;
        }
    }
}