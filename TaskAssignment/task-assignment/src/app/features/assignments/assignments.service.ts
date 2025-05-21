import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin, map, Observable } from 'rxjs';
import { ListAssignedTasksForUserItem } from '../assignments/list-assigned-tasks-for-user-item';
import { AssignTasksCommand } from './assign-tasks.command';

@Injectable({
  providedIn: 'root'
})
export class AssignmentsService {
  private readonly baseUrl = 'https://localhost:7059/api/assignments';

  constructor(private http: HttpClient) { }

  getAssignedTasks(userId: string): Observable<ListAssignedTasksForUserItem[]> {
    // TODO: obecnie w zasadzie na sztywno pobieramy wszytskie zadania na kilku żądaniach stronicowania - docelowo należałoby głowny widok UI rodzielić 
    // na widok wyświetlania i widok przypisywania zadań
    const pageSize = 10;
    const maxTasks = 11;
    const totalPages = Math.ceil(maxTasks / pageSize);

    const requests: Observable<ListAssignedTasksForUserItem[]>[] = [];

    for (let page = 0; page < totalPages; page++) {
      const req = this.http.get<ListAssignedTasksForUserItem[]>(
        `${this.baseUrl}/assigned-for-user`,
        { params: { userId, page: page.toString() } }
      );
      requests.push(req);
    }

    return forkJoin(requests).pipe(
      map(pages => pages.flat())
    );
  }


  assignTasks(command: AssignTasksCommand): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/assign-for-user`, command);
  }
}
