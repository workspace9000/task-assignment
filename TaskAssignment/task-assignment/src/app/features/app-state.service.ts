import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { AssignedTaskVm } from './assignment/assigned-task.vm';
import { AvailableTaskVm } from './tasks/available-task.vm';
import { ListAllUsersItem } from './users/list-all-users-item';
import { AssignmentsService } from './assignments/assignments.service';
import { TasksService } from './tasks/tasks.service';

@Injectable({
  providedIn: 'root'
})
export class AppStateService {
  constructor(
    private tasksService: TasksService,
    private assignmentsService: AssignmentsService
  ) { }

  private availableTasksCache = new Map<number, AvailableTaskVm[]>();
  private availableTasksPagination = {
    page: 0,
    pageSize: 10,
    availableTasksTotalCount: 0,
    tasks$: new BehaviorSubject<AvailableTaskVm[]>([])
  };

  private loadAvailableTasksPage(): void {
    const user = this.selectedUserSubject.getValue();
    const page = this.availableTasksPagination.page;
    if (!user) return;

    const assignedNewIds = new Set(
      this.assignedTasksSubject.getValue()
        .filter(t => t.isNew)
        .map(t => t.id)
    );

    const emitPageWithFlags = (raw: AvailableTaskVm[]) => {
      const pageWithFlags = raw.map(task => ({
        ...task,
        isDisabled: assignedNewIds.has(task.id)
      }));
      this.availableTasksPagination.tasks$.next(pageWithFlags);
    };

    if (this.availableTasksCache.has(page)) {
      emitPageWithFlags(this.availableTasksCache.get(page)!);
      return;
    }

    this.tasksService.getAvailableTasks(user.id, page).subscribe({
      next: tasks => {
        this.availableTasksPagination.availableTasksTotalCount = tasks.totalPages;
        const cacheable = tasks.items.map(t => ({ ...t, isDisabled: false }));
        this.availableTasksCache.set(page, cacheable);
        emitPageWithFlags(cacheable);
      },
      error: err => console.error('Błąd ładowania zadań:', err)
    });
  }

  nextAvailableTasksPage(): void {
    var reqPage = this.availableTasksPagination.page + 2;
    if (reqPage > this.availableTasksPagination.availableTasksTotalCount) {
      return;
    }
    this.availableTasksPagination.page++;
    this.loadAvailableTasksPage();
  }

  prevAvailableTasksPage(): void {
    if (this.availableTasksPagination.page > 0) {
      this.availableTasksPagination.page--;
      this.loadAvailableTasksPage();
    }
  }

  private resetAvailableTasksPagination(): void {
    this.availableTasksPagination.page = 0;
  }

  // -----


  private selectedUserSubject = new BehaviorSubject<ListAllUsersItem | null>(null);
  selectedUser$ = this.selectedUserSubject.asObservable();

  private assignedTasksSubject = new BehaviorSubject<AssignedTaskVm[]>([]);
  assignedTasks$ = this.assignedTasksSubject.asObservable();

  get availableTasks$(): Observable<AvailableTaskVm[]> {
    return this.availableTasksPagination.tasks$.asObservable();
  }

  private hasUnsavedChangesSubject = new BehaviorSubject<boolean>(false);
  hasUnsavedChanges$ = this.hasUnsavedChangesSubject.asObservable();

  private usersSubject = new BehaviorSubject<ListAllUsersItem[]>([]);
  users$ = this.usersSubject.asObservable();

  setUsers(users: ListAllUsersItem[]): void {
    this.usersSubject.next(users);
  }

  setSelectedUser(user: ListAllUsersItem | null): void {
    this.selectedUserSubject.next(user);
  }

  setAssignedTasks(tasks: AssignedTaskVm[]): void {
    this.assignedTasksSubject.next(tasks);
  }

  addAssignedTask(task: AssignedTaskVm): void {
    const current = this.assignedTasksSubject.getValue();
    this.assignedTasksSubject.next([...current, task]);
  }

  removeAssignedTask(taskId: string): void {
    const updated = this.assignedTasksSubject.getValue().filter(t => t.id !== taskId);
    this.assignedTasksSubject.next(updated);
  }

  setHasUnsavedChanges(value: boolean): void {
    this.hasUnsavedChangesSubject.next(value);
  }

  private resetAll(): void {
    this.selectedUserSubject.next(null);
    this.assignedTasksSubject.next([]);
    this.hasUnsavedChangesSubject.next(false);
    this.availableTasksCache.clear();
    this.resetAvailableTasksPagination();
  }

  changeSelectedUser(userId: string): void {
    const user = this.usersSubject.getValue().find(u => u.id === userId) || null;
    if (!user) return;

    this.selectedUserSubject.next(user);
    this.hasUnsavedChangesSubject.next(false);
    this.availableTasksCache.clear();

    this.reloadTasksForUser(user.id);
  }


  assignTask(taskId: string): void {
    const user = this.selectedUserSubject.getValue();
    if (!user) return;

    const currentPage = this.availableTasksPagination.tasks$.getValue();
    const assigned = this.assignedTasksSubject.getValue();

    const task = currentPage.find(t => t.id === taskId);
    if (!task) return;

    if (!this.isAssignmentValid(task, assigned, user)) return;

    const updatedPage = currentPage.map(t =>
      t.id === taskId ? { ...t, isDisabled: true } : t
    );

    this.availableTasksPagination.tasks$.next(updatedPage);

    const cached = this.availableTasksCache.get(this.availableTasksPagination.page);
    if (cached) {
      this.availableTasksCache.set(
        this.availableTasksPagination.page,
        cached.map(t =>
          t.id === taskId ? { ...t, isDisabled: true } : t
        )
      );
    }

    const newAssigned: AssignedTaskVm = {
      ...task,
      isNew: true
    };

    this.assignedTasksSubject.next([...assigned, newAssigned]);
    this.hasUnsavedChangesSubject.next(true);
  }


  confirmAssignments(): void {
    if (!this.hasUnsavedChangesSubject.getValue()) return;

    if (!this.validateFinalAssignments()) return;

    const user = this.selectedUserSubject.getValue();
    const assigned = this.assignedTasksSubject.getValue();

    if (!user || assigned.length === 0) return;

    const taskIds = assigned.map(t => t.id);
    const command = { userId: user.id, taskIds };

    this.assignmentsService.assignTasks(command).subscribe({
      next: () => {
        this.availableTasksCache.clear();
        this.resetAvailableTasksPagination()
        this.reloadTasksForUser(user.id);
      },
      error: (err) => {
        console.error('Błąd podczas przypisywania zadań', err);
      }
    });
  }

  private reloadTasksForUser(userId: string): void {
    if (!userId) return;

    this.resetAvailableTasksPagination();
    this.loadAvailableTasksPage();

    this.assignmentsService.getAssignedTasks(userId).subscribe({
      next: assigned => {
        const assignedVm: AssignedTaskVm[] = assigned.map(task => ({ ...task, isNew: false }));
        this.assignedTasksSubject.next(assignedVm);
      },
      error: err => {
        console.error('Błąd pobierania przypisanych zadań:', err);
      }
    });
  }


  private isAssignmentValid(
    candidate: AvailableTaskVm,
    assigned: AssignedTaskVm[],
    user: ListAllUsersItem
  ): boolean {
    const updated = [...assigned, candidate];

    // R2: limit maksymalny
    if (updated.length > 11) {
      window.alert('Nie można przypisać więcej niż 11 zadań.');
      return false;
    }

    // R3, R4: rola użytkownika vs typ zadania
    const isDev = user.role === 'Developer';
    const isOps = user.role === 'DevOps' || user.role === 'Administrator';

    if (isDev && candidate.type !== 'Implementation') {
      window.alert('Programista może mieć przypisane tylko zadania typu Implementacja.');
      return false;
    }

    if (!isDev && !isOps) {
      window.alert(`Rola użytkownika "${user.role}" nie ma uprawnień do przypisywania zadań.`);
      return false;
    }

    return true;
  }


  private validateFinalAssignments(): boolean {
    const assigned = this.assignedTasksSubject.getValue();
    const user = this.selectedUserSubject.getValue();

    if (!user || assigned.length === 0) return false;

    // R2: liczba przypisań
    if (assigned.length < 5 || assigned.length > 11) {
      window.alert('Liczba przypisanych zadań musi zawierać się w przedziale 5–11.');
      return false;
    }

    // R5–R7: rozkład trudności
    const total = assigned.length;
    const count = (levels: number[]) => assigned.filter(t => levels.includes(t.difficulty)).length;
    const perc = (val: number) => (val / total) * 100;

    const hard = perc(count([4, 5]));
    const easy = perc(count([1, 2]));
    const mid = perc(count([3]));

    if (hard < 10 || hard > 30) {
      window.alert('Zadania o trudności 4–5 muszą stanowić 10–30% przypisań.');
      return false;
    }

    if (easy > 50) {
      window.alert('Zadania o trudności 1–2 mogą stanowić maksymalnie 50% przypisań.');
      return false;
    }

    if (mid > 90) {
      window.alert('Zadania o trudności 3 mogą stanowić maksymalnie 90% przypisań.');
      return false;
    }

    return true;
  }


}
