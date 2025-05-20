import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ListAllUsersItem } from './list-all-users-item';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private readonly baseUrl = 'https://localhost:7059/api/users';

  constructor(private http: HttpClient) { }

  getUsers(): Observable<ListAllUsersItem[]> {
    return this.http.get<ListAllUsersItem[]>(this.baseUrl);
  }
}
