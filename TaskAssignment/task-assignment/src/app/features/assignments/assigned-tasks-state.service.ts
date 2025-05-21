import { Injectable } from '@angular/core';
import { BehaviorSubject, EMPTY, map, Observable, tap } from 'rxjs';
import { AssignTasksCommand } from '../assignments/assign-tasks.command';
import { AssignmentsService } from '../assignments/assignments.service';
import { ListAllUsersItem } from '../users/list-all-users-item';
import { AssignedTaskVm } from './assigned-task.vm';
import { AssignmentValidationService } from './assignment-validation.service';
import { AvailableTaskVm } from '../tasks/available-task.vm';

@Injectable({
  providedIn: 'root'
})
export class AssignedTasksStateService {
  private assignedTasksSubject = new BehaviorSubject<AssignedTaskVm[]>([]);
  assignedTasks$ = this.assignedTasksSubject.asObservable();

  private hasUnsavedChangesSubject = new BehaviorSubject<boolean>(false);
  hasUnsavedChanges$ = this.hasUnsavedChangesSubject.asObservable();

  constructor(
    private assignmentValidation: AssignmentValidationService,
    private assignmentsService: AssignmentsService
  ) { }

  setAssignedTasks(tasks: AssignedTaskVm[]): void {
    this.assignedTasksSubject.next(tasks);
    this.clearUnsavedChanges();
  }

  addAssignedTask(task: AssignedTaskVm): void {
    const current = this.assignedTasksSubject.getValue();
    this.assignedTasksSubject.next([...current, task]);
    this.markUnsavedChanges();
  }

  removeAssignedTask(taskId: string): void {
    const updated = this.assignedTasksSubject.getValue().filter(t => t.id !== taskId);
    this.assignedTasksSubject.next(updated);
    this.markUnsavedChanges();
  }

  validateAssignments(): boolean {
    const tasks = this.assignedTasksSubject.getValue();
    return this.assignmentValidation.validateAssignmentRules(tasks);
  }

  confirmAssignments(user: ListAllUsersItem | null): Observable<void> {
    if (!this.hasUnsavedChangesSubject.getValue()) return EMPTY;
    if (!user) return EMPTY;

    const tasks = this.assignedTasksSubject.getValue();
    if (tasks.length === 0) return EMPTY;
    if (!this.validateAssignments()) return EMPTY;

    const taskIds = tasks.map(t => t.id);
    const command: AssignTasksCommand = { userId: user.id, taskIds };

    return this.assignmentsService.assignTasks(command).pipe(
      tap(() => this.clearUnsavedChanges())
    );
  }

  hasUnsavedChanges(): boolean {
    return this.hasUnsavedChangesSubject.getValue();
  }

  loadAssignments(userId: string): Observable<AssignedTaskVm[]> {
    return this.assignmentsService.getAssignedTasks(userId).pipe(
      map(assigned => assigned.map(t => ({ ...t, isNew: false })))
    );
  }

  isAssignmentAllowed(task: AvailableTaskVm, user: ListAllUsersItem): boolean {
    const assigned = this.assignedTasksSubject.getValue();
    return this.assignmentValidation.isAssignmentAllowed(task, assigned, user);
  }

  markUnsavedChanges(): void {
    this.hasUnsavedChangesSubject.next(true);
  }

  clearUnsavedChanges(): void {
    this.hasUnsavedChangesSubject.next(false);
  }
}
