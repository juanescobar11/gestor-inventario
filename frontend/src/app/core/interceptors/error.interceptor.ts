import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

interface ProblemDetails {
  title?: string;
  detail?: string;
}

export const errorInterceptor: HttpInterceptorFn = (req, next) =>
  next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const problema = error.error as ProblemDetails | null;
      const mensaje =
        error.status === 0
          ? 'No hay conexión con el servidor. Verifica que la API esté en ejecución.'
          : problema?.detail ?? problema?.title ?? 'Ocurrió un error inesperado.';
      return throwError(() => new Error(mensaje));
    }),
  );