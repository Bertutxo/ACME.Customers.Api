ACME.Customers.Api

API REST para gestionar Clientes y Comerciales de ACME.

✔️ Propósito

Solución de ejemplo para el proceso de selección como Especialista .NET en ACME:

Arquitectura en capas (Core, Infrastructure, Application, Api)

Patrones Repository + Unit of Work

DTOs y AutoMapper

Validaciones con FluentValidation

Base de datos SQLite local con seed

📂 Estructura de proyectos

ACME.Customers.Core: Entidades de dominio e interfaces de repositorios/UoW

ACME.Customers.Infrastructure: EF Core DbContext, repositorios e implementación de UnitOfWork

ACME.Customers.Application: Servicios de aplicación, caso de uso, DTOs, mapeos y validaciones

ACME.Customers.Api: ASP.NET Core Web API, controladores, Swagger/OpenAPI y arranque

🚀 Requisitos

.NET 8.0 SDK (o .NET 6/7 según tu versión)

Visual Studio 2022/2023 o VS Code

🛠️ Cómo ejecutar

Clona el repositorio:

git clone https://github.com/<tuUsuario>/ACME.Customers.Api.git
cd ACME.Customers.Api

Restaura paquetes y compila:

dotnet restore
dotnet build

Ejecuta la API:

dotnet run --project ACME.Customers.Api/ACME.Customers.Api.csproj

Abre Swagger en tu navegador: https://localhost:5001
