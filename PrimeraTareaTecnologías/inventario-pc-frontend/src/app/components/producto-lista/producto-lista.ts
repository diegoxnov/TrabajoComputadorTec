import { Component, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Producto as ProductoService } from '../../services/producto';
import { ProductoDto } from '../../models/producto.model';
import { obtenerNombreCategoria } from '../../utils/inventario.utils';

@Component({
  selector: 'app-producto-lista',
  imports: [RouterLink, DecimalPipe, FormsModule],
  templateUrl: './producto-lista.html',
  styleUrl: './producto-lista.css',
})
export class ProductoLista implements OnInit {
  productos = signal<ProductoDto[]>([]);
  categorias = signal<string[]>([]);
  filtroTexto = signal('');

  constructor(private productoService: ProductoService) {}

  ngOnInit(): void {
    this.cargarProductos();
    this.productoService.obtenerCategorias().subscribe((categorias) => this.categorias.set(categorias));
  }

  cargarProductos(): void {
    this.productoService.obtenerTodos().subscribe((productos) => this.productos.set(productos));
  }

  nombreCategoria(categoria: number): string {
    return obtenerNombreCategoria(this.categorias(), categoria);
  }

  /**
   * Función STATEFUL: modifica el estado del componente (signal `filtroTexto`)
   * cada vez que el usuario escribe en el buscador.
   */
  actualizarFiltro(texto: string): void {
    this.filtroTexto.set(texto);
  }

  productosFiltrados(): ProductoDto[] {
    const filtro = this.filtroTexto().trim().toLowerCase();
    if (!filtro) return this.productos();
    return this.productos().filter(
      (p) => p.nombre.toLowerCase().includes(filtro) || p.marca.toLowerCase().includes(filtro)
    );
  }

  eliminar(id: number): void {
    if (!confirm('¿Eliminar este producto?')) return;
    this.productoService.eliminar(id).subscribe(() => this.cargarProductos());
  }
}
