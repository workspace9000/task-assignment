import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { PaginationManager } from '../../core/pagination/pagination-manager';
import { ListAllUsersItem } from '../users/list-all-users-item';
import { AvailableTaskVm } from './available-task.vm';
import { TasksService } from './tasks.service';


@Injectable({
  providedIn: 'root'
})
export class AvailableTasksStateService {
  private selectedUserSubject = new BehaviorSubject<ListAllUsersItem | null>(null);
  private newlyAssignedTaskIds: Set<string> = new Set();

  constructor(private tasksService: TasksService) { }

  private pager = new PaginationManager<AvailableTaskVm>(page => {
    const user = this.selectedUserSubject.getValue();
    if (!user) return of({ items: [], totalPages: 0 });

    return this.tasksService.getAvailableTasks(user.id, page).pipe(
      map(response => ({
        items: response.items.map(task => ({
          ...task,
          isDisabled: this.isTaskNewlyAssigned(task.id)
        })),
        totalPages: response.totalPages
      }))
    );
  });

  get availableTasks$(): Observable<AvailableTaskVm[]> {
    return this.pager.page$;
  }

  getCurrentPageItems(): AvailableTaskVm[] {
    return this.pager.getCurrentPageItems();
  }


  nextPage(): void {
    this.pager.nextPage();
  }

  prevPage(): void {
    this.pager.prevPage();
  }

  updateItem(predicate: (item: AvailableTaskVm) => boolean, updater: (item: AvailableTaskVm) => AvailableTaskVm): void {
    this.pager.updateItem(predicate, updater);
  }

  reset(): void {
    this.pager.reset();
  }

  invalidateCache(): void {
    this.pager.invalidateCache();
  }

  setSelectedUser(user: ListAllUsersItem | null): void {
    this.selectedUserSubject.next(user);
    this.invalidateCache();
    this.reset();
  }

  markTaskAsAssigned(taskId: string): void {
    this.newlyAssignedTaskIds.add(taskId);
    this.updateItem(t => t.id === taskId, t => ({ ...t, isDisabled: true }));
  }

  clearAssignedMarks(): void {
    this.newlyAssignedTaskIds.clear();
  }

  private isTaskNewlyAssigned(taskId: string): boolean {
    return this.newlyAssignedTaskIds.has(taskId);
  }
}
