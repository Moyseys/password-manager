import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainNavigator } from './main-navigator';

describe('MainNavigator', () => {
  let component: MainNavigator;
  let fixture: ComponentFixture<MainNavigator>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MainNavigator]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MainNavigator);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
