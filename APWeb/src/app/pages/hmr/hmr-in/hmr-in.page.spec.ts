import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HmrInPage } from './hmr-in.page';

describe('HmrInPage', () => {
  let component: HmrInPage;
  let fixture: ComponentFixture<HmrInPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(HmrInPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
