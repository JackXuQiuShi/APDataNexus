import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CreatePOPage } from './create-po.page';

describe('CreatePOPage', () => {
  let component: CreatePOPage;
  let fixture: ComponentFixture<CreatePOPage>;

  beforeEach(() => {
    fixture = TestBed.createComponent(CreatePOPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
