import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InventoryReturnPage } from './inventory-return.page';

describe('InventoryReturnPage', () => {
  let component: InventoryReturnPage;
  let fixture: ComponentFixture<InventoryReturnPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(InventoryReturnPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
