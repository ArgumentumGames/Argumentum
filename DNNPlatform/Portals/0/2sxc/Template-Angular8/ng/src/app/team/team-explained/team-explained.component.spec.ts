import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamExplainedComponent } from './team-explained.component';

describe('TeamExplainedComponent', () => {
  let component: TeamExplainedComponent;
  let fixture: ComponentFixture<TeamExplainedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TeamExplainedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TeamExplainedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
