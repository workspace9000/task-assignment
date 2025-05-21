import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { AssignedTasksListComponent } from './features/assignments/components/assigned-tasks-list/assigned-tasks-list.component';
import { AssignmentContainerComponent } from './features/assignments/components/assignment-container/assignment-container.component';
import { AvailableTasksListComponent } from './features/assignments/components/available-tasks-list/available-tasks-list.component';
import { SelectUserHeaderComponent } from './features/assignments/components/select-user-header/select-user-header.component';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


@NgModule({
    declarations: [
        AppComponent,
        AssignmentContainerComponent,
        SelectUserHeaderComponent,
        AssignedTasksListComponent,
        AvailableTasksListComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        SharedModule,
        CoreModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
