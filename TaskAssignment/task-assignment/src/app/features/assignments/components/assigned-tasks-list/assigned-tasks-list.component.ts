import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AssignedTaskVm } from '../../assigned-task.vm';
import { AppStateService } from '../../../app-state.service';


@Component({
  selector: 'app-assigned-tasks-list',
  templateUrl: './assigned-tasks-list.component.html',
  styleUrls: ['./assigned-tasks-list.component.scss']
})
export class AssignedTasksListComponent implements OnInit {
  tasks$!: Observable<AssignedTaskVm[]>;

  constructor(private appState: AppStateService) { }

  ngOnInit(): void {
    this.tasks$ = this.appState.assignedTasks$;
  }
}
