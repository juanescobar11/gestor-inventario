export interface Producto {
  id: number;
  nombre: string;
  descripcion: string | null;
  precio: number;
  stock: number;
  stockMinimo: number;
  categoriaId: number;
  categoriaNombre: string;
  stockBajo: boolean;
  fechaCreacion: string;
}

export interface GuardarProductoRequest {
  nombre: string;
  descripcion: string | null;
  precio: number;
  stock: number;
  stockMinimo: number;
  categoriaId: number;
}

export type EstadoStock = 'todos' | 'bajo' | 'normal';