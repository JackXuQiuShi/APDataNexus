import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InventoryLocationPage } from './inventory-location.page';

describe('InventoryLocationPage', () => {
  let component: InventoryLocationPage;
  let fixture: ComponentFixture<InventoryLocationPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(InventoryLocationPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
