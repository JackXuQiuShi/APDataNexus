import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ItemInsertPage } from './item-insert.page';

describe('ItemInsertPage', () => {
  let component: ItemInsertPage;
  let fixture: ComponentFixture<ItemInsertPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(ItemInsertPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
