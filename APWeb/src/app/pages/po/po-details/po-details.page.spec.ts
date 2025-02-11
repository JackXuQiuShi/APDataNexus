import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PoDetailsPage } from './po-details.page';

describe('PoDetailsPage', () => {
  let component: PoDetailsPage;
  let fixture: ComponentFixture<PoDetailsPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(PoDetailsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
