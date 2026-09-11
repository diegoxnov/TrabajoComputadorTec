import { ProductoDto } from '../models/producto.model';

/**
 * Función STATELESS: dado un arreglo de categorías y un índice, devuelve el nombre.
 * No depende de `this` ni de ningún estado externo: mismos argumentos -> mismo resultado.
 */
export function obtenerNombreCategoria(categorias: string[], categoria: number): string {
  return categorias[categoria] ?? categoria.toString();
}

/**
 * Función STATELESS: calcula el precio total de los productos seleccionados.
 * Recibe todo lo que necesita por parámetro, no lee ni modifica estado externo.
 */
export function calcularPrecioTotalSeleccion(
  productos: ProductoDto[],
  productoIdsSeleccionados: (number | null)[]
): number {
  return productoIdsSeleccionados
    .filter((id): id is number => id != null)
    .reduce((total, id) => {
      const producto = productos.find((p) => p.id === id);
      return total + (producto?.precio ?? 0);
    }, 0);
}
