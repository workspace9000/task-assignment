import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AppStateService } from '../../../app-state.service';
import { AvailableTaskVm } from '../../../tasks/available-task.vm';

@Component({
  selector: 'app-available-tasks-list',
  templateUrl: './available-tasks-list.component.html',
  styleUrls: ['./available-tasks-list.component.scss']
})
export class AvailableTasksListComponent implements OnInit {
  tasks$!: Observable<AvailableTaskVm[]>;

  constructor(private appState: AppStateService) { }

  ngOnInit(): void {
    this.tasks$ = this.appState.availableTasks$;
  }

  onAssign(taskId: string): void {
    this.appState.assignTask(taskId);
  }
}
