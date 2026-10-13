import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { API_URL } from '../../../core/config/api.config';
import { EstadoStock, GuardarProductoRequest, Producto } from '../models/producto.model';

@Injectable({ providedIn: 'root' })
export class ProductosService {
  private readonly http = inject(HttpClient);
  private readonly url = `${API_URL}/productos`;

  listar(categoriaId: number | null, estado: EstadoStock): Observable<Producto[]> {
    let params = new HttpParams();
    if (categoriaId !== null) {
      params = params.set('categoriaId', categoriaId);
    }
    if (estado !== 'todos') {
      params = params.set('stockBajo', estado === 'bajo');
    }
    return this.http.get<Producto[]>(this.url, { params });
  }

  obtenerPorId(id: number): Observable<Producto> {
    return this.http.get<Producto>(`${this.url}/${id}`);
  }

  crear(request: GuardarProductoRequest): Observable<Producto> {
    return this.http.post<Producto>(this.url, request);
  }

  actualizar(id: number, request: GuardarProductoRequest): Observable<Producto> {
    return this.http.put<Producto>(`${this.url}/${id}`, request);
  }

  eliminar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}