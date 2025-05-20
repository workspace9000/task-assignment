import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ListAvailableTasksForUserItem } from './list-available-tasks-for-user-item';

@Injectable({
  providedIn: 'root'
})
export class TasksService {
  private readonly baseUrl = 'https://localhost:7059/api/tasks';

  constructor(private http: HttpClient) { }

  getAvailableTasks(userId: string): Observable<ListAvailableTasksForUserItem[]> {
    return this.http.get<ListAvailableTasksForUserItem[]>(
      `${this.baseUrl}/available`,
      { params: { userId } }
    );
  }
}
