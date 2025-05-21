import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { AssignedTaskVm } from './assignments/assigned-task.vm';
import { AvailableTaskVm } from './tasks/available-task.vm';
import { ListAllUsersItem } from './users/list-all-users-item';
import { AssignedTasksStateService } from './assignments/assigned-tasks-state.service';
import { AvailableTasksStateService } from './tasks/available-tasks-state.service';

@Injectable({
  providedIn: 'root'
})
export class AppStateService {
  private selectedUserSubject = new BehaviorSubject<ListAllUsersItem | null>(null);
  selectedUser$ = this.selectedUserSubject.asObservable();

  private usersSubject = new BehaviorSubject<ListAllUsersItem[]>([]);
  users$ = this.usersSubject.asObservable();

  constructor(
    private availableTasksState: AvailableTasksStateService,
    private assignedTasksState: AssignedTasksStateService
  ) { }

  get availableTasks$(): Observable<AvailableTaskVm[]> {
    return this.availableTasksState.availableTasks$;
  }

  get assignedTasks$(): Observable<AssignedTaskVm[]> {
    return this.assignedTasksState.assignedTasks$;
  }

  get hasUnsavedChanges$(): Observable<boolean> {
    return this.assignedTasksState.hasUnsavedChanges$;
  }

  setUsers(users: ListAllUsersItem[]): void {
    this.usersSubject.next(users);
  }

  changeSelectedUser(userId: string): void {
    const user = this.usersSubject.getValue().find(u => u.id === userId) || null;
    if (!user) return;

    this.selectedUserSubject.next(user);
    this.assignedTasksState.clearUnsavedChanges();
    this.availableTasksState.setSelectedUser(user);

    this.reloadAssignmentsForUser(user.id);
  }

  assignTask(taskId: string): void {
    const user = this.selectedUserSubject.getValue();
    if (!user) return;

    const currentPage = this.availableTasksState.getCurrentPageItems();
    const task = currentPage.find(t => t.id === taskId);
    if (!task) return;

    if (!this.assignedTasksState.isAssignmentAllowed(task, user)) return;

    this.availableTasksState.markTaskAsAssigned(task.id);
    this.assignedTasksState.addAssignedTask({ ...task, isNew: true });
  }

  confirmAssignments(): void {
    const user = this.selectedUserSubject.getValue();
    if (!user) return;

    this.assignedTasksState.confirmAssignments(user).subscribe({
      next: () => {
        this.availableTasksState.invalidateCache();
        this.availableTasksState.reset();
        this.reloadAssignmentsForUser(user.id);
      },
      error: err => {
        console.error('Błąd podczas przypisywania zadań', err);
      }
    });
  }

  nextAvailableTasksPage(): void {
    this.availableTasksState.nextPage();
  }

  prevAvailableTasksPage(): void {
    this.availableTasksState.prevPage();
  }

  private reloadAssignmentsForUser(userId: string): void {
    this.availableTasksState.clearAssignedMarks();
    this.assignedTasksState.clearUnsavedChanges();

    this.assignedTasksState.loadAssignments(userId).subscribe({
      next: vm => this.assignedTasksState.setAssignedTasks(vm),
      error: err => console.error('Błąd pobierania przypisanych zadań:', err)
    });
  }
}
