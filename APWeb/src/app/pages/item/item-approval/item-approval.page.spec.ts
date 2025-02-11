import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ItemApprovalPage } from './item-approval.page';

describe('ItemApprovalPage', () => {
  let component: ItemApprovalPage;
  let fixture: ComponentFixture<ItemApprovalPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(ItemApprovalPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
