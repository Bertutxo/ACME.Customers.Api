# ACME.Customers.Api

API REST para gestionar Clientes y Comerciales de ACME, con UI ligera basada en HTMX + Alpine.js + Tailwind.

## ✔️ Propósito

- Arquitectura en capas: Core, Infrastructure, Application, Api.
- Patrón Repository + Unit of Work.
- DTOs y AutoMapper.
- Validaciones con FluentValidation.
- Base de datos SQLite local con seed.
- UI ligera con HTMX + Alpine.js + Tailwind (opcional) para integrarse sin front pesado.

## 📂 Estructura de proyectos

- **ACME.Customers.Core**  
- **ACME.Customers.Infrastructure**  
- **ACME.Customers.Application**  
- **ACME.Customers.Api**  
  - `Program.cs` / arranque: configura DbContext, servicios, Swagger, Static Files.
  - `Controllers/`: JSON endpoints y fragmentos HTML para HTMX.
  - `wwwroot/`: aquí va la UI estática (index.html, assets, etc.).
  - `appsettings.json`: cadena de conexión SQLite.

## 🚀 Requisitos

- .NET 8.0 SDK (o 6/7 según tu entorno).
- Visual Studio 2022/2023 o VS Code.
- (Opcional) Node.js si deseas compilar CSS con Tailwind CLI para producción.

## 🛠️ Cómo ejecutar

1. **Clonar y compilar API**  
   ```bash
   git clone https://github.com/<tuUsuario>/ACME.Customers.Api.git
   cd ACME.Customers.Api
   dotnet restore
   dotnet build