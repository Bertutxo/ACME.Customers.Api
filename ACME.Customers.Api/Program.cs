using System.Diagnostics;
using ACME.Customers.Application.DependencyInjection;
using ACME.Customers.Infrastructure;
using ACME.Customers.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Mostrar en consola la ConnectionString leída
Debug.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection")
    is string cs ? $"[DBG] ConnStr: {cs}" : "[ERR] No hay DefaultConnection");

// 1. Configurar servicios

// a) DbContext (SQLite)
builder.Services.AddDbContext<CustomersDbContext>(opts =>
    opts.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// b) Capa de infra y aplicación
builder.Services.AddInfrastructure();  // IClientRepository, IUnitOfWork, etc.
builder.Services.AddApplication();     // IClientService, AutoMapper, validadores, etc.

// c) API + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Crear esquema si no existe (no borra datos)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CustomersDbContext>();
    var path = db.Database.GetDbConnection().DataSource;
    Console.WriteLine($"[DBG] SQLite DB file: {Path.GetFullPath(path)}");

    db.Database.EnsureCreated();
}

// 3. Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ACME.Customers API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();