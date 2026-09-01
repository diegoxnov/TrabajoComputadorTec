import { Component, OnInit, signal } from '@angular/core';
import { DecimalPipe, DatePipe } from '@angular/common';
import { Armado as ArmadoService } from '../../services/armado';
import { ArmadoDto } from '../../models/armado.model';

@Component({
  selector: 'app-armados-historial',
  imports: [DecimalPipe, DatePipe],
  templateUrl: './armados-historial.html',
  styleUrl: './armados-historial.css',
})
export class ArmadosHistorial implements OnInit {
  armados = signal<ArmadoDto[]>([]);

  constructor(private armadoService: ArmadoService) {}

  ngOnInit(): void {
    this.armadoService.obtenerTodos().subscribe((armados) => this.armados.set(armados));
  }
}
