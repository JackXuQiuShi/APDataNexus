import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ItemChangePricePage } from './item-change-price.page';

describe('ItemChangePricePage', () => {
  let component: ItemChangePricePage;
  let fixture: ComponentFixture<ItemChangePricePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ItemChangePricePage]
    }).compileComponents();

    fixture = TestBed.createComponent(ItemChangePricePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
