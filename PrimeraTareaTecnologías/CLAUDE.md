# Inventario de Componentes de PC — App Web N-Capas

## Objetivo

Trabajo universitario: aplicación web simple aplicando arquitectura n-capas. Sin rúbrica ni tecnología obligatoria — el criterio es demostrar la separación en capas con un CRUD funcional. Prioridad: **simple y rápido de desarrollar**, no perfecto. No sobre-ingenierizar (sin auth, sin paginación, sin tests exhaustivos, sin patrones adicionales que no aporten al punto de la tarea).

## Idea de negocio

Inventario de productos de computadora (CPU, GPU, RAM, Almacenamiento, Placa Madre, Fuente de Poder, Gabinete). Un "Armado" es la selección de un producto por categoría para formar una PC completa. Al confirmar el armado, se descuenta 1 unidad de stock de cada producto usado. Si algún producto seleccionado no tiene stock, se rechaza el armado completo (todo o nada, en una transacción).

## Stack

- **Backend:** ASP.NET Core Web API (.NET 8/9/10 — usar la versión de SDK que ya tengas instalada)
- **ORM:** Entity Framework Core + Npgsql (Postgres)
- **Base de datos:** PostgreSQL vía Supabase (reutilizar el approach ya resuelto en el proyecto Biblioteca2026: NuGet `Npgsql.EntityFrameworkCore.PostgreSQL`, y ojo con el modo del connection pooler de Supabase — usar el puerto **5432 (Session mode)**, no el 6543 (Transaction mode), porque EF Core Migrations necesita prepared statements)
- **Frontend:** Angular (standalone components, sin necesidad de NgModules) + `HttpClient`

## Arquitectura (n-capas)

```
InventarioPC.sln
├── InventarioPC.Domain        → Entidades puras, enums. Sin dependencias de nada.
├── InventarioPC.DataAccess    → DbContext, Repositorios, Migraciones. Depende de Domain.
├── InventarioPC.Business      → Servicios con las reglas de negocio, DTOs. Depende de DataAccess + Domain.
├── InventarioPC.Api           → Controllers, Program.cs, DI, CORS, Swagger. Depende de Business.
└── inventario-pc-frontend/    → Proyecto Angular aparte (SPA). Consume la Api por HTTP.
```

Regla de dependencia: cada capa solo conoce a la de abajo. Angular → Api → Business → DataAccess → Domain/DB.

## Modelo de datos

```csharp
// Domain
public enum Categoria { Cpu, Gpu, Ram, Almacenamiento, PlacaMadre, FuenteDePoder, Gabinete }

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public Categoria Categoria { get; set; }
    public string Marca { get; set; } = "";
    public decimal Precio { get; set; }
    public int Stock { get; set; }
}

public class Armado
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public DateTime FechaCreacion { get; set; }
    public decimal PrecioTotal { get; set; }
    public List<ArmadoDetalle> Detalles { get; set; } = new();
}

public class ArmadoDetalle
{
    public int Id { get; set; }
    public int ArmadoId { get; set; }
    public Armado Armado { get; set; } = null!;
    public int ProductoId { get; set; }
    public Producto Producto { get; set; } = null!;
    public int Cantidad { get; set; } = 1;
}
```

No hace falta más que esto. `ArmadoDetalle` es una tabla puente simple (no una relación fija por categoría), así el modelo queda flexible sin agregar complejidad real.

## Reglas de negocio (capa Business)

`ArmadoService.CrearArmadoAsync(List<int> productoIds)`:
1. Cargar los productos por Id.
2. Validar que todos existan y que `Stock >= 1` cada uno. Si alguno falla, lanzar excepción de negocio (ej. `StockInsuficienteException`) — no se crea nada.
3. Dentro de una transacción de EF Core (`BeginTransactionAsync`): crear el `Armado`, un `ArmadoDetalle` por producto, descontar `Stock -= 1` en cada `Producto`, calcular `PrecioTotal` sumando precios.
4. `SaveChangesAsync` + commit.

`ProductoService`: CRUD directo, sin reglas especiales más allá de validar que `Stock >= 0` y `Precio >= 0`.

## Endpoints API

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/productos` | Lista todos |
| GET | `/api/productos/{id}` | Detalle |
| POST | `/api/productos` | Crear |
| PUT | `/api/productos/{id}` | Actualizar |
| DELETE | `/api/productos/{id}` | Eliminar |
| GET | `/api/productos/categorias` | Devuelve los valores del enum `Categoria` (para llenar los `<select>` del frontend) |
| GET | `/api/armados` | Lista historial |
| GET | `/api/armados/{id}` | Detalle con productos incluidos |
| POST | `/api/armados` | Body: `{ "nombre": "...", "productoIds": [1,5,9,...] }` → crea el armado y descuenta stock |

Devolver `400 BadRequest` con el mensaje de la excepción cuando falla la validación de stock (no hace falta un manejo de errores más sofisticado).

## Frontend Angular (vistas simples, sin diseño elaborado)

Componentes standalone, routing básico:

- `ProductoListaComponent` (`/productos`) — tabla con Nombre, Categoría, Marca, Precio, Stock, botones Editar/Eliminar/Nuevo
- `ProductoFormComponent` (`/productos/nuevo`, `/productos/:id/editar`) — formulario reactivo simple
- `ArmarPcComponent` (`/armar`) — un `<select>` por categoría (solo productos con stock > 0), muestra precio total en vivo, botón "Armar" → POST
- `ArmadosHistorialComponent` (`/armados`) — lista simple de armados pasados

Un solo `ProductoService` y un `ArmadoService` en Angular que envuelven `HttpClient`. Nada de NgRx ni state management — no aporta a una app de este tamaño.

## Setup rápido

```bash
# Backend
dotnet new sln -n InventarioPC
dotnet new classlib -n InventarioPC.Domain
dotnet new classlib -n InventarioPC.DataAccess
dotnet new classlib -n InventarioPC.Business
dotnet new webapi -n InventarioPC.Api
dotnet sln add **/*.csproj
# referencias entre proyectos: Api -> Business -> DataAccess -> Domain

cd InventarioPC.DataAccess
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design

# Frontend
ng new inventario-pc-frontend --routing --style=css --ssr=false
```

Connection string de Supabase va en `InventarioPC.Api/appsettings.Development.json` (este archivo debe estar en `.gitignore` si el repo es público, para no subir la contraseña):

```json
{ "ConnectionStrings": { "Default": "Host=<host-supabase>;Port=5432;Database=postgres;Username=postgres;Password=<tu-password>" } }
```

CORS en `Program.cs` de la Api: habilitar el origin de Angular (`http://localhost:4200`) sin restricciones adicionales — es un proyecto académico, no hace falta endurecerlo.

## Fuera de alcance (a propósito)

- Sin autenticación/usuarios
- Sin eliminar armados (evita la lógica de "reponer stock")
- Sin paginación ni filtros avanzados
- Sin tests automatizados (a menos que se pida explícitamente)

## Definición de "terminado"

1. `dotnet run` en Api levanta Swagger y los 9 endpoints responden.
2. `ng serve` levanta el frontend y las 4 pantallas funcionan contra la Api local.
3. Se puede crear un producto, editarlo, armar una PC y ver cómo el stock baja, y el armado queda en el historial.
