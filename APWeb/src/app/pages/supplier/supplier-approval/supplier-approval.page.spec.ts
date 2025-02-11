import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SupplierApprovalPage } from './supplier-approval.page';

describe('SupplierApprovalPage', () => {
  let component: SupplierApprovalPage;
  let fixture: ComponentFixture<SupplierApprovalPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(SupplierApprovalPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
