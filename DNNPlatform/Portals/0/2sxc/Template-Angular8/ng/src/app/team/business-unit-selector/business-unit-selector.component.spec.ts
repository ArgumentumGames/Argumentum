import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BusinessUnitSelectorComponent } from './business-unit-selector.component';

describe('BusinessUnitSelectorComponent', () => {
  let component: BusinessUnitSelectorComponent;
  let fixture: ComponentFixture<BusinessUnitSelectorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BusinessUnitSelectorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BusinessUnitSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
