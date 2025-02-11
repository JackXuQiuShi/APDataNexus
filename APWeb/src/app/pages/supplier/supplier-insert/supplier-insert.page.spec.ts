import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SupplierInsertPage } from './supplier-insert.page';

describe('SupplierInsertPage', () => {
  let component: SupplierInsertPage;
  let fixture: ComponentFixture<SupplierInsertPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(SupplierInsertPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
