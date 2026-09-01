export interface ArmadoDetalleDto {
  productoId: number;
  productoNombre: string;
  precio: number;
  cantidad: number;
}

export interface ArmadoDto {
  id: number;
  nombre: string;
  fechaCreacion: string;
  precioTotal: number;
  detalles: ArmadoDetalleDto[];
}

export interface ArmadoCrearDto {
  nombre: string;
  productoIds: number[];
}
