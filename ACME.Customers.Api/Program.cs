using ACME.Customers.Application.DependencyInjection;
using ACME.Customers.Infrastructure;
using ACME.Customers.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1) Registrar servicios
builder.Services
    .AddDbContext<CustomersDbContext>(o =>
        o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddInfrastructure()   // IClientRepository, ISalesRepRepository, IUnitOfWork
    .AddApplication();      // IClientService, ISalesRepService, AutoMapper, FluentValidation

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2) Crear la base de datos si no existe (sin borrar datos)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CustomersDbContext>();
    db.Database.EnsureCreated();
}

// 3) Servir la UI estática desde wwwroot
//    wwwroot/index.html + assets (Tailwind/HTMX/Alpine/etc.)
app.UseDefaultFiles();  // URL “/” → wwwroot/index.html
app.UseStaticFiles();   // wwwroot/**

// 4) Swagger solo en Development
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ACME.Customers API V1");
        c.RoutePrefix = "swagger"; // UI de Swagger en /swagger
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// 5) Fallback SPA: cualquier otra ruta sirve index.html
app.MapFallbackToFile("index.html");

app.Run();