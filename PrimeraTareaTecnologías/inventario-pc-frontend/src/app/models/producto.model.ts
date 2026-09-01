export interface ProductoDto {
  id: number;
  nombre: string;
  categoria: number;
  marca: string;
  precio: number;
  stock: number;
}

export interface ProductoCrearDto {
  nombre: string;
  categoria: number;
  marca: string;
  precio: number;
  stock: number;
}
