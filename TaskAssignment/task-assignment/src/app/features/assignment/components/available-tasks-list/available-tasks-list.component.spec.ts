import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvailableTasksListComponent } from './available-tasks-list.component';

describe('AvailableTasksListComponent', () => {
  let component: AvailableTasksListComponent;
  let fixture: ComponentFixture<AvailableTasksListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AvailableTasksListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AvailableTasksListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
