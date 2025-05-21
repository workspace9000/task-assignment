import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AppStateService } from '../../../app-state.service';
import { ListAllUsersItem } from '../../../users/list-all-users-item';
import { MatSelectChange } from '@angular/material/select';

@Component({
  selector: 'app-select-user-header',
  templateUrl: './select-user-header.component.html',
  styleUrls: ['./select-user-header.component.scss']
})
export class SelectUserHeaderComponent implements OnInit {
  users$!: Observable<ListAllUsersItem[]>;
  selectedUser$!: Observable<ListAllUsersItem | null>;
  hasUnsavedChanges$!: Observable<boolean>;

  selectedUserId: string = "";

  constructor(private appState: AppStateService) { }

  ngOnInit(): void {
    this.selectedUser$ = this.appState.selectedUser$;
    this.hasUnsavedChanges$ = this.appState.hasUnsavedChanges$;
    this.users$ = this.appState.users$;

    this.selectedUser$.subscribe(user => {
      this.selectedUserId = user?.id ?? "";
    });
  }

  onUserChange(event: MatSelectChange): void {
    const userId = event.value;
    if (!userId) return;
    this.appState.changeSelectedUser(userId);
  }

  onConfirm(): void {
    this.appState.confirmAssignments();
  }
}
