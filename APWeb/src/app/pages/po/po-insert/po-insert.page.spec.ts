import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PoInsertPage } from './po-insert.page';

describe('PoInsertPage', () => {
  let component: PoInsertPage;
  let fixture: ComponentFixture<PoInsertPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(PoInsertPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
