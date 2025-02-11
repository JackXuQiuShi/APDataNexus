import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SupplierUpdatePage } from './supplier-update.page';

describe('SupplierUpdatePage', () => {
  let component: SupplierUpdatePage;
  let fixture: ComponentFixture<SupplierUpdatePage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(SupplierUpdatePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
