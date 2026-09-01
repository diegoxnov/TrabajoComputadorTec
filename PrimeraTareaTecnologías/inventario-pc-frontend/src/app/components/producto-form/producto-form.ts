import { Component, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Producto as ProductoService } from '../../services/producto';

@Component({
  selector: 'app-producto-form',
  imports: [ReactiveFormsModule],
  templateUrl: './producto-form.html',
  styleUrl: './producto-form.css',
})
export class ProductoForm implements OnInit {
  categorias = signal<string[]>([]);
  productoId: number | null = null;
  form: ReturnType<FormBuilder['group']>;

  constructor(
    private fb: FormBuilder,
    private productoService: ProductoService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.form = this.fb.group({
      nombre: ['', Validators.required],
      categoria: [0, Validators.required],
      marca: ['', Validators.required],
      precio: [0, [Validators.required, Validators.min(0)]],
      stock: [0, [Validators.required, Validators.min(0)]],
    });
  }

  ngOnInit(): void {
    this.productoService.obtenerCategorias().subscribe((categorias) => this.categorias.set(categorias));

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.productoId = Number(idParam);
      this.productoService.obtenerPorId(this.productoId).subscribe((producto) => {
        this.form.patchValue(producto);
      });
    }
  }

  guardar(): void {
    if (this.form.invalid) return;
    const valores = this.form.getRawValue() as {
      nombre: string;
      categoria: number | string;
      marca: string;
      precio: number | string;
      stock: number | string;
    };
    const dto = {
      nombre: valores.nombre,
      categoria: Number(valores.categoria),
      marca: valores.marca,
      precio: Number(valores.precio),
      stock: Number(valores.stock),
    };

    const accion: Observable<unknown> = this.productoId
      ? this.productoService.actualizar(this.productoId, dto)
      : this.productoService.crear(dto);

    accion.subscribe(() => this.router.navigate(['/productos']));
  }
}
