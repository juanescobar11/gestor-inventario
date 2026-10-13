import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductosListado } from './productos-listado';

describe('ProductosListado', () => {
  let component: ProductosListado;
  let fixture: ComponentFixture<ProductosListado>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductosListado],
    }).compileComponents();

    fixture = TestBed.createComponent(ProductosListado);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
