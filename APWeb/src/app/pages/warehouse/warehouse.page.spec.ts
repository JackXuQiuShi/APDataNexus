import { ComponentFixture, TestBed } from '@angular/core/testing';
import { WarehousePage } from './warehouse.page';

describe('WarehousePage', () => {
  let component: WarehousePage;
  let fixture: ComponentFixture<WarehousePage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(WarehousePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
