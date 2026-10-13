import { Routes } from '@angular/router';

export const PRODUCTOS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./components/productos-listado/productos-listado').then((m) => m.ProductosListado),
  },
  {
    path: 'nuevo',
    loadComponent: () =>
      import('./components/producto-form/producto-form').then((m) => m.ProductoForm),
  },
  {
    path: ':id/editar',
    loadComponent: () =>
      import('./components/producto-form/producto-form').then((m) => m.ProductoForm),
  },
];