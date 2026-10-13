# Gestor de Inventario

Aplicación full stack para gestionar productos de una pequeña empresa: CRUD de productos, asociación a categorías, visibilidad de stock bajo y ajuste de stock.

- **Backend:** .NET 10, Clean Architecture + CQRS (MediatR), Entity Framework Core, SQL Server, Swagger.
- **Frontend:** Angular 20+ (standalone, zoneless, Signals, OnPush), Angular Material, SCSS + BEM.

## Requisitos

- .NET SDK 10
- Node.js 20.19 o superior y Angular CLI (`npm install -g @angular/cli`)
- SQL Server o SQL Server Express

## 1. Base de datos

Ejecutar el script `database/GestorInventarioDB.sql` en SQL Server (crea la base `GestorInventarioDB`, las tablas y las 3 categorías iniciales).

La API también ejecuta un seed al iniciar: si no hay productos, inserta productos de ejemplo.

## 2. Backend

1. Ajustar la cadena de conexión en `backend/src/GestorInventario.Api/appsettings.json` (por defecto apunta a `localhost\SQLEXPRESS` con autenticación de Windows).
2. Ejecutar:

```bash
cd backend/src/GestorInventario.Api
dotnet run --urls http://localhost:5080
```

3. Swagger: http://localhost:5080/swagger

## 3. Frontend

```bash
cd frontend
npm install
ng serve
```

Abrir http://localhost:4200 (el backend debe estar corriendo en el puerto 5080; la URL base se configura en `frontend/src/app/core/config/api.config.ts`).

## Endpoints principales

| Método | Ruta | Descripción |
|---|---|---|
| GET | /api/categorias | Lista las categorías activas |
| GET | /api/productos?categoriaId=&stockBajo= | Lista con filtros opcionales |
| GET | /api/productos/stock-bajo | Productos con Stock < StockMinimo |
| GET | /api/productos/{id} | Obtiene un producto |
| POST | /api/productos | Crea un producto (201) |
| PUT | /api/productos/{id} | Edita un producto |
| PATCH | /api/productos/{id}/stock | Ajusta stock (Entrada / Salida) |
| DELETE | /api/productos/{id} | Elimina un producto (204) |

## Arquitectura

**Backend** (`backend/src`): la dependencia va siempre hacia adentro.

```
Api -> Application -> Domain
Infrastructure -> Application -> Domain
```

- `Domain`: entidades y reglas de negocio (Precio > 0, Stock >= 0, nombre obligatorio).
- `Application`: Commands, Queries, Handlers (MediatR) y puertos (`IProductoRepository`).
- `Infrastructure`: `PersistenceContext` (EF Core), repositorios y seed.
- `Api`: controladores, middleware de errores y `Program.cs`.

**Frontend** (`frontend/src/app`): `core/` (config, interceptors), `shared/` (componentes reutilizables), `features/productos/` (componentes, servicios, modelos y rutas con lazy loading).

## Cuestionario técnico

Las respuestas están en [RESPUESTAS.md](RESPUESTAS.md).