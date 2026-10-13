import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'productos', pathMatch: 'full' },
  {
    path: 'productos',
    loadChildren: () => import('./features/productos/routes').then((m) => m.PRODUCTOS_ROUTES),
  },
];