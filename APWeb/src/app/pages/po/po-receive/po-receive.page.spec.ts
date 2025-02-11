import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PoReceivePage } from './po-receive.page';

describe('PoReceivePage', () => {
  let component: PoReceivePage;
  let fixture: ComponentFixture<PoReceivePage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(PoReceivePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
