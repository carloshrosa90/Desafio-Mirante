using Desafio.Aplicacao.Interface;
using Desafio.Aplicacao.Service;
using Desafio.Infrastructure.Persistence;
using Desafio.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Desafio.Apresentacao",
		Version = "v1"
	});
	options.EnableAnnotations();
});

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection"),
		sql => sql.MigrationsAssembly("Desafio.Apresentacao")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IStatus, StatusService>();
builder.Services.AddScoped<ITarefa, TarefaService>();

var app = builder.Build();

if (string.Equals(Environment.GetEnvironmentVariable("APPLY_MIGRATIONS_AT_STARTUP"), "true", StringComparison.OrdinalIgnoreCase))
{
	using var scope = app.Services.CreateScope();
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	if (db.Database.GetPendingMigrations().Any())
		db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "Desafio.Apresentacao v1");
	});
}

// Imagem aspnet define DOTNET_RUNNING_IN_CONTAINER=true; redirecionar para HTTPS quebra o Swagger em http://+:8080
if (!string.Equals(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), "true", StringComparison.OrdinalIgnoreCase))
	app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
