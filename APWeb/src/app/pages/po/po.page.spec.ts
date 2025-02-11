import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PoPage } from './po.page';

describe('PoPage', () => {
  let component: PoPage;
  let fixture: ComponentFixture<PoPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(PoPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
