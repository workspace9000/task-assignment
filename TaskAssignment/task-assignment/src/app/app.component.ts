import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AppStateService } from './features/app-state.service';

@Component({
  selector: 'app-root',
  standalone: false,
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit, OnDestroy {
  private hasUnsaved = false;
  private sub!: Subscription;

  title = 'task-assignment';

  constructor(private appState: AppStateService) { }

  ngOnInit(): void {
    this.sub = this.appState.hasUnsavedChanges$.subscribe(flag => {
      this.hasUnsaved = flag;
    });
  }

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: BeforeUnloadEvent): void {
    if (this.hasUnsaved) {
      $event.preventDefault();
      $event.returnValue = ''; // wymagane przez przeglądarki
    }
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
