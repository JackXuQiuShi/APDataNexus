import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InventoryReturnListPage } from './inventory-return-list.page';

describe('InventoryReturnListPage', () => {
  let component: InventoryReturnListPage;
  let fixture: ComponentFixture<InventoryReturnListPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(InventoryReturnListPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
