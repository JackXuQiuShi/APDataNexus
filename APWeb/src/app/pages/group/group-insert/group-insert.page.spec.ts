import { ComponentFixture, TestBed } from '@angular/core/testing';
import { GroupInsertPage } from './group-insert.page';

describe('GroupInsertPage', () => {
  let component: GroupInsertPage;
  let fixture: ComponentFixture<GroupInsertPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(GroupInsertPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
