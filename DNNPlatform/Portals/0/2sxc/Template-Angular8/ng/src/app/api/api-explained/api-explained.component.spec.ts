import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApiExplainedComponent } from './api-explained.component';

describe('ApiExplainedComponent', () => {
  let component: ApiExplainedComponent;
  let fixture: ComponentFixture<ApiExplainedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApiExplainedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApiExplainedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
