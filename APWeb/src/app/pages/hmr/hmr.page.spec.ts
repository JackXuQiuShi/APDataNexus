import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HmrPage } from './hmr.page';

describe('HmrPage', () => {
  let component: HmrPage;
  let fixture: ComponentFixture<HmrPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(HmrPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
