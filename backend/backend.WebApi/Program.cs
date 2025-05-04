using backend.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services
	.AddRepositories()
	.AddServices()
	.AddValidators();

builder.Services.AddPostgresDbContext(builder.Configuration.GetConnectionStringOrThrow("Postgres"));

builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionMiddleware();

app.UseDatabaseMigration();

app.UseScalar(app.Environment);

app.UseHttpsRedirection();

app.MapControllers();

app.Run();