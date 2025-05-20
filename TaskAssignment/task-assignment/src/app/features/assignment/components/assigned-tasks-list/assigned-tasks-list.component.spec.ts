import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignedTasksListComponent } from './assigned-tasks-list.component';

describe('AssignedTasksListComponent', () => {
  let component: AssignedTasksListComponent;
  let fixture: ComponentFixture<AssignedTasksListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssignedTasksListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssignedTasksListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
