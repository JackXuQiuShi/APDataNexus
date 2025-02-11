import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ItemDraftPage } from './item-draft.page';

describe('ItemDraftPage', () => {
  let component: ItemDraftPage;
  let fixture: ComponentFixture<ItemDraftPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(ItemDraftPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
