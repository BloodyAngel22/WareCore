using backend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace backend.WebApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseScalar(this WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.MapScalarApiReference("docs", options => options.WithTheme(ScalarTheme.DeepSpace));
                app.MapOpenApi();
            }

            return app;
        }

        public static void UseDatabaseMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying migrations: {ex.Message}");
                throw;
            }
        }
    }
}