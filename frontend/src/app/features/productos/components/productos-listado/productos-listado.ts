import { CurrencyPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, OnInit, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';

import {
  ConfirmDialog,
  ConfirmDialogData,
} from '../../../../shared/components/confirm-dialog/confirm-dialog';
import { Categoria } from '../../models/categoria.model';
import { EstadoStock, Producto } from '../../models/producto.model';
import { CategoriasService } from '../../services/categorias.service';
import { ProductosService } from '../../services/productos.service';

@Component({
  selector: 'app-productos-listado',
  imports: [
    RouterLink,
    CurrencyPipe,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    MatFormFieldModule,
    MatProgressBarModule,
  ],
  templateUrl: './productos-listado.html',
  styleUrl: './productos-listado.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductosListado implements OnInit {
  private readonly productosService = inject(ProductosService);
  private readonly categoriasService = inject(CategoriasService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  readonly columnas = ['nombre', 'categoria', 'precio', 'stock', 'estado', 'acciones'];

  readonly productos = signal<Producto[]>([]);
  readonly categorias = signal<Categoria[]>([]);
  readonly categoriaId = signal<number | null>(null);
  readonly estadoStock = signal<EstadoStock>('todos');
  readonly cargando = signal(false);

  readonly totalStockBajo = computed(() => this.productos().filter((p) => p.stockBajo).length);
  readonly sinResultados = computed(() => !this.cargando() && this.productos().length === 0);

  ngOnInit(): void {
    this.categoriasService.listar().subscribe({
      next: (lista) => this.categorias.set(lista),
      error: (err: Error) => this.notificar(err.message),
    });
    this.cargar();
  }

  cambiarCategoria(categoriaId: number | null): void {
    this.categoriaId.set(categoriaId);
    this.cargar();
  }

  cambiarEstado(estado: EstadoStock): void {
    this.estadoStock.set(estado);
    this.cargar();
  }

  eliminar(producto: Producto): void {
    const data: ConfirmDialogData = {
      titulo: 'Eliminar producto',
      mensaje: `¿Eliminar "${producto.nombre}"? Esta acción no se puede deshacer.`,
    };

    this.dialog
      .open<ConfirmDialog, ConfirmDialogData, boolean>(ConfirmDialog, { data })
      .afterClosed()
      .subscribe((confirmado) => {
        if (!confirmado) {
          return;
        }
        this.productosService.eliminar(producto.id).subscribe({
          next: () => {
            this.notificar('Producto eliminado');
            this.cargar();
          },
          error: (err: Error) => this.notificar(err.message),
        });
      });
  }

  private cargar(): void {
    this.cargando.set(true);
    this.productosService.listar(this.categoriaId(), this.estadoStock()).subscribe({
      next: (lista) => {
        this.productos.set(lista);
        this.cargando.set(false);
      },
      error: (err: Error) => {
        this.cargando.set(false);
        this.notificar(err.message);
      },
    });
  }

  private notificar(mensaje: string): void {
    this.snackBar.open(mensaje, 'Cerrar', { duration: 4000 });
  }
}