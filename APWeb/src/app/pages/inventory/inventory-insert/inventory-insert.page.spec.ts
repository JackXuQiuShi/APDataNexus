import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InventoryInsertPage } from './inventory-insert.page';

describe('InventoryInsertPage', () => {
  let component: InventoryInsertPage;
  let fixture: ComponentFixture<InventoryInsertPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(InventoryInsertPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
