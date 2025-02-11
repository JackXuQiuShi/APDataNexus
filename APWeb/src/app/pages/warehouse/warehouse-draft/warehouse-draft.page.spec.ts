import { ComponentFixture, TestBed } from '@angular/core/testing';
import { WarehouseDraftPage } from './warehouse-draft.page';

describe('WarehouseDraftPage', () => {
  let component: WarehouseDraftPage;
  let fixture: ComponentFixture<WarehouseDraftPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(WarehouseDraftPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
