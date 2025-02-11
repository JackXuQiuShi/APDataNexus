import { ComponentFixture, TestBed } from '@angular/core/testing';
import { WarehouseInPage } from './warehouse-in.page';

describe('WarehouseInPage', () => {
  let component: WarehouseInPage;
  let fixture: ComponentFixture<WarehouseInPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(WarehouseInPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
