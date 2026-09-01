import { Component, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DecimalPipe } from '@angular/common';
import { Producto as ProductoService } from '../../services/producto';
import { Armado as ArmadoService } from '../../services/armado';
import { ProductoDto } from '../../models/producto.model';

@Component({
  selector: 'app-armar-pc',
  imports: [FormsModule, DecimalPipe],
  templateUrl: './armar-pc.html',
  styleUrl: './armar-pc.css',
})
export class ArmarPc implements OnInit {
  categorias = signal<string[]>([]);
  productos = signal<ProductoDto[]>([]);
  nombreArmado = '';
  seleccion: Record<number, number | null> = {};
  mensajeError = signal<string | null>(null);
  mensajeExito = signal<string | null>(null);

  precioTotal(): number {
    return Object.values(this.seleccion)
      .filter((id): id is number => id != null)
      .reduce((total, id) => {
        const producto = this.productos().find((p) => p.id === id);
        return total + (producto?.precio ?? 0);
      }, 0);
  }

  constructor(
    private productoService: ProductoService,
    private armadoService: ArmadoService
  ) {}

  ngOnInit(): void {
    this.productoService.obtenerCategorias().subscribe((categorias) => {
      this.categorias.set(categorias);
      categorias.forEach((_, i) => (this.seleccion[i] = null));
    });
    this.productoService.obtenerTodos().subscribe((productos) => this.productos.set(productos));
  }

  productosPorCategoria(categoria: number): ProductoDto[] {
    return this.productos().filter((p) => p.categoria === categoria && p.stock > 0);
  }

  armar(): void {
    this.mensajeError.set(null);
    this.mensajeExito.set(null);

    const productoIds = Object.values(this.seleccion).filter((id): id is number => id != null);

    if (productoIds.length === 0) {
      this.mensajeError.set('Selecciona al menos un producto.');
      return;
    }

    this.armadoService.crear({ nombre: this.nombreArmado || 'Armado sin nombre', productoIds }).subscribe({
      next: () => {
        this.mensajeExito.set('¡Armado creado con éxito!');
        this.nombreArmado = '';
        this.seleccion = {};
        this.categorias().forEach((_, i) => (this.seleccion[i] = null));
        this.productoService.obtenerTodos().subscribe((productos) => this.productos.set(productos));
      },
      error: (err) => {
        this.mensajeError.set(err.error ?? 'Ocurrió un error al armar la PC.');
      },
    });
  }
}
