import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PoDraftPage } from './po-draft.page';

describe('PoDraftPage', () => {
  let component: PoDraftPage;
  let fixture: ComponentFixture<PoDraftPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(PoDraftPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
