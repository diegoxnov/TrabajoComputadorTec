# TrabajoComputadorTec

## Primer trabajo
### Código: vhHsYqa6VJO54kNY

## Descripción del proyecto

Aplicación web de **Inventario de Componentes de PC**, desarrollada con arquitectura n-capas:

- **Backend:** ASP.NET Core Web API (.NET) + Entity Framework Core + PostgreSQL (Supabase)
- **Frontend:** Angular (standalone components, signals) + Bootstrap 5
- **Capas backend:** `InventarioPC.Domain` → `InventarioPC.DataAccess` → `InventarioPC.Business` → `InventarioPC.Api`

Un "Armado" es la selección de un producto por categoría para formar una PC completa; al confirmarlo se descuenta stock de cada producto usado (todo o nada, en una transacción).

El código fuente está en [`PrimeraTareaTecnologías/`](PrimeraTareaTecnologías), y el frontend Angular en [`PrimeraTareaTecnologías/inventario-pc-frontend/`](PrimeraTareaTecnologías/inventario-pc-frontend).

## Funciones Stateful y Stateless

En el frontend Angular se distinguen dos tipos de funciones:

- **Stateless (sin estado):** funciones puras que reciben todos sus datos por parámetro, no leen ni modifican nada externo (ni `this`, ni signals). Con los mismos argumentos siempre devuelven el mismo resultado, por lo que son fáciles de testear de forma aislada.
- **Stateful (con estado):** funciones que dependen o modifican el estado interno del componente (signals, propiedades de clase), y suelen producir efectos secundarios (llamadas HTTP, actualización de la UI).

### Funciones Stateless (2)

Ubicadas en [`inventario-pc-frontend/src/app/utils/inventario.utils.ts`](PrimeraTareaTecnologías/inventario-pc-frontend/src/app/utils/inventario.utils.ts) — no existían en el proyecto original, se agregaron para esta entrega:

| Función | Descripción |
|---|---|
| `obtenerNombreCategoria(categorias, categoria)` | Devuelve el nombre de una categoría dado el arreglo de categorías y su índice. Usada en [`producto-lista.ts`](PrimeraTareaTecnologías/inventario-pc-frontend/src/app/components/producto-lista/producto-lista.ts) (método `nombreCategoria`). |
| `calcularPrecioTotalSeleccion(productos, productoIdsSeleccionados)` | Calcula el precio total de los productos seleccionados. Usada en [`armar-pc.ts`](PrimeraTareaTecnologías/inventario-pc-frontend/src/app/components/armar-pc/armar-pc.ts) (método `precioTotal`). |

### Funciones Stateful (2)

Ya existían varias en el proyecto original (por ejemplo `cargarProductos()`, `eliminar()`, `guardar()`, `armar()`), todas modifican signals del componente. Para esta entrega se agregaron dos nuevas, ligadas a funcionalidad nueva de la UI:

| Función | Ubicación | Descripción |
|---|---|---|
| `actualizarFiltro(texto)` | [`producto-lista.ts`](PrimeraTareaTecnologías/inventario-pc-frontend/src/app/components/producto-lista/producto-lista.ts) | Actualiza el signal `filtroTexto` cada vez que el usuario escribe en el buscador de productos (nuevo campo de búsqueda por nombre/marca en la tabla). |
| `limpiarSeleccion()` | [`armar-pc.ts`](PrimeraTareaTecnologías/inventario-pc-frontend/src/app/components/armar-pc/armar-pc.ts) | Resetea el estado del formulario de armado: nombre, selección por categoría y mensajes de error/éxito (nuevo botón "Limpiar"). |

## Vista con Bootstrap

Se integró **Bootstrap 5** vía CDN en [`index.html`](PrimeraTareaTecnologías/inventario-pc-frontend/src/index.html) (CSS + bundle JS, sin necesidad de instalar dependencias npm adicionales). Se mejoraron:

- **Navbar** responsiva ([`app.html`](PrimeraTareaTecnologías/inventario-pc-frontend/src/app/app.html)) con enlaces a Productos, Armar PC e Historial.
- **Lista de productos**: tabla con estilos `table-striped`/`table-hover`, badges de stock (verde/rojo) y un buscador en vivo.
- **Formulario de producto**: inputs y selects con clases `form-control` / `form-select`.
- **Armar PC**: tarjetas (`card`) por categoría, alertas Bootstrap para mensajes de error/éxito y botón para limpiar la selección.
- **Historial de armados**: tarjetas con lista de detalles y badge de precio total.

## Cómo correr el proyecto

```bash
# Backend
cd PrimeraTareaTecnologías
dotnet run --project InventarioPC.Api

# Frontend
cd PrimeraTareaTecnologías/inventario-pc-frontend
npm install
ng serve
```

El backend expone Swagger y 9 endpoints (`/api/productos`, `/api/armados`); el frontend corre en `http://localhost:4200` y consume la API local.

## Estado de la base de datos

La base de datos PostgreSQL en Supabase (proyecto `ComputadorasTecDeConstruction`) está **pausada**, por lo que actualmente no es posible probar el backend contra datos reales ni correr las migraciones de EF Core. El frontend se verificó de forma aislada (sin backend) y funciona correctamente con Bootstrap; falta validar el flujo completo end-to-end (crear producto, armar PC, ver historial) una vez que la base de datos esté disponible.
