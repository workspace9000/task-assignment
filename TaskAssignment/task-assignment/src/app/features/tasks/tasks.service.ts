import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ListAvailableTasksForUser } from './ListAvailableTasksForUser';

@Injectable({
  providedIn: 'root'
})
export class TasksService {
  private readonly baseUrl = 'https://localhost:7059/api/tasks';

  constructor(private http: HttpClient) { }

  getAvailableTasks(userId: string, page: number): Observable<ListAvailableTasksForUser> {
    return this.http.get<ListAvailableTasksForUser>(
      `${this.baseUrl}/available`,
      {
        params: {
          userId,
          page: page.toString()
        }
      }
    );
  }

}
