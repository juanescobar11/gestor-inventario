import { ChangeDetectionStrategy, Component, OnInit, computed, inject, input, signal } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';

import { Categoria } from '../../models/categoria.model';
import { GuardarProductoRequest } from '../../models/producto.model';
import { CategoriasService } from '../../services/categorias.service';
import { ProductosService } from '../../services/productos.service';

@Component({
  selector: 'app-producto-form',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatProgressBarModule,
  ],
  templateUrl: './producto-form.html',
  styleUrl: './producto-form.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductoForm implements OnInit {
  // Llega desde la ruta (:id) gracias a withComponentInputBinding()
  readonly id = input<string>();

  private readonly fb = inject(NonNullableFormBuilder);
  private readonly productosService = inject(ProductosService);
  private readonly categoriasService = inject(CategoriasService);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  readonly categorias = signal<Categoria[]>([]);
  readonly cargando = signal(false);
  readonly guardando = signal(false);
  readonly titulo = computed(() => (this.id() === undefined ? 'Nuevo producto' : 'Editar producto'));

  readonly form = this.fb.group({
    nombre: ['', [Validators.required, Validators.maxLength(150)]],
    descripcion: ['', [Validators.maxLength(500)]],
    precio: [0, [Validators.required, Validators.min(0.01)]],
    stock: [0, [Validators.required, Validators.min(0)]],
    stockMinimo: [5, [Validators.required, Validators.min(0)]],
    categoriaId: [null as number | null, [Validators.required]],
  });

  ngOnInit(): void {
    this.categoriasService.listar().subscribe({
      next: (lista) => this.categorias.set(lista),
      error: (err: Error) => this.notificar(err.message),
    });

    const id = this.id();
    if (id !== undefined) {
      this.cargando.set(true);
      this.productosService.obtenerPorId(Number(id)).subscribe({
        next: (producto) => {
          this.form.patchValue({
            nombre: producto.nombre,
            descripcion: producto.descripcion ?? '',
            precio: producto.precio,
            stock: producto.stock,
            stockMinimo: producto.stockMinimo,
            categoriaId: producto.categoriaId,
          });
          this.cargando.set(false);
        },
        error: (err: Error) => {
          this.cargando.set(false);
          this.notificar(err.message);
        },
      });
    }
  }

  guardar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { categoriaId, descripcion, ...resto } = this.form.getRawValue();
    if (categoriaId === null) {
      return;
    }

    const request: GuardarProductoRequest = {
      ...resto,
      descripcion: descripcion.trim() === '' ? null : descripcion.trim(),
      categoriaId,
    };

    const id = this.id();
    const peticion$ =
      id === undefined
        ? this.productosService.crear(request)
        : this.productosService.actualizar(Number(id), request);

    this.guardando.set(true);
    peticion$.subscribe({
      next: () => {
        this.notificar('Producto guardado');
        this.router.navigate(['/productos']);
      },
      error: (err: Error) => {
        this.guardando.set(false);
        this.notificar(err.message);
      },
    });
  }

  private notificar(mensaje: string): void {
    this.snackBar.open(mensaje, 'Cerrar', { duration: 4000 });
  }
}