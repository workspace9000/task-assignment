import { Component, OnInit } from '@angular/core';
import { switchMap } from 'rxjs';
import { AppStateService } from '../../../app-state.service';
import { UsersService } from '../../../users/users.service';
import { TasksService } from '../../../tasks/tasks.service';
import { AssignmentsService } from '../../../assignments/assignments.service';

@Component({
  selector: 'app-assignment-container',
  templateUrl: './assignment-container.component.html',
  styleUrl: './assignment-container.component.scss'
})
export class AssignmentContainerComponent implements OnInit {

  constructor(
    private usersService: UsersService,
    private tasksService: TasksService,
    private assignmentsService: AssignmentsService,
    private appState: AppStateService
  ) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  private loadUsers(): void {
    this.usersService.getUsers().subscribe(users => {
      this.appState.setUsers(users);
    });
  }
}

