import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ItemUpdatePage } from './item-update.page';

describe('ItemUpdatePage', () => {
  let component: ItemUpdatePage;
  let fixture: ComponentFixture<ItemUpdatePage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(ItemUpdatePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
