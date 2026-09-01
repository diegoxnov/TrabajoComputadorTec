import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ArmadoCrearDto, ArmadoDto } from '../models/armado.model';

const API_URL = 'http://localhost:5199/api/armados';

@Injectable({
  providedIn: 'root',
})
export class Armado {
  constructor(private http: HttpClient) {}

  obtenerTodos(): Observable<ArmadoDto[]> {
    return this.http.get<ArmadoDto[]>(API_URL);
  }

  obtenerPorId(id: number): Observable<ArmadoDto> {
    return this.http.get<ArmadoDto>(`${API_URL}/${id}`);
  }

  crear(dto: ArmadoCrearDto): Observable<ArmadoDto> {
    return this.http.post<ArmadoDto>(API_URL, dto);
  }
}
