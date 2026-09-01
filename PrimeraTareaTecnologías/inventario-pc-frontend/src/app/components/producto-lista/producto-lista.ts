import { Component, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DecimalPipe } from '@angular/common';
import { Producto as ProductoService } from '../../services/producto';
import { ProductoDto } from '../../models/producto.model';

@Component({
  selector: 'app-producto-lista',
  imports: [RouterLink, DecimalPipe],
  templateUrl: './producto-lista.html',
  styleUrl: './producto-lista.css',
})
export class ProductoLista implements OnInit {
  productos = signal<ProductoDto[]>([]);
  categorias = signal<string[]>([]);

  constructor(private productoService: ProductoService) {}

  ngOnInit(): void {
    this.cargarProductos();
    this.productoService.obtenerCategorias().subscribe((categorias) => this.categorias.set(categorias));
  }

  cargarProductos(): void {
    this.productoService.obtenerTodos().subscribe((productos) => this.productos.set(productos));
  }

  nombreCategoria(categoria: number): string {
    return this.categorias()[categoria] ?? categoria.toString();
  }

  eliminar(id: number): void {
    if (!confirm('¿Eliminar este producto?')) return;
    this.productoService.eliminar(id).subscribe(() => this.cargarProductos());
  }
}
