import { ComponentFixture, TestBed } from '@angular/core/testing';
import { WarehouseInventoryPage } from './warehouse-inventory.page';

describe('WarehouseInventoryPage', () => {
  let component: WarehouseInventoryPage;
  let fixture: ComponentFixture<WarehouseInventoryPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(WarehouseInventoryPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
