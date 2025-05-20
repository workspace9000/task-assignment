import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ListAssignedTasksForUserItem } from '../assignment/list-assigned-tasks-for-user-item';
import { AssignTasksCommand } from './assign-tasks.command';

@Injectable({
  providedIn: 'root'
})
export class AssignmentsService {
  private readonly baseUrl = 'https://localhost:7059/api/assignments';

  constructor(private http: HttpClient) { }

  getAssignedTasks(userId: string): Observable<ListAssignedTasksForUserItem[]> {
    return this.http.get<ListAssignedTasksForUserItem[]>(
      `${this.baseUrl}/assigned-for-user`,
      { params: { userId } }
    );
  }

  assignTasks(command: AssignTasksCommand): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/assign-for-user`, command);
  }
}
