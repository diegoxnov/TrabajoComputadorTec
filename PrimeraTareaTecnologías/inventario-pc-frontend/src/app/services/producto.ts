import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductoCrearDto, ProductoDto } from '../models/producto.model';

const API_URL = 'http://localhost:5199/api/productos';

@Injectable({
  providedIn: 'root',
})
export class Producto {
  constructor(private http: HttpClient) {}

  obtenerTodos(): Observable<ProductoDto[]> {
    return this.http.get<ProductoDto[]>(API_URL);
  }

  obtenerPorId(id: number): Observable<ProductoDto> {
    return this.http.get<ProductoDto>(`${API_URL}/${id}`);
  }

  obtenerCategorias(): Observable<string[]> {
    return this.http.get<string[]>(`${API_URL}/categorias`);
  }

  crear(dto: ProductoCrearDto): Observable<ProductoDto> {
    return this.http.post<ProductoDto>(API_URL, dto);
  }

  actualizar(id: number, dto: ProductoCrearDto): Observable<void> {
    return this.http.put<void>(`${API_URL}/${id}`, dto);
  }

  eliminar(id: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/${id}`);
  }
}
