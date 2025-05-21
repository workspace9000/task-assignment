import { Injectable } from "@angular/core";
import { AvailableTaskVm } from "../tasks/available-task.vm";
import { ListAllUsersItem } from "../users/list-all-users-item";
import { AssignedTaskVm } from "./assigned-task.vm";

@Injectable({
  providedIn: 'root'
})
export class AssignmentValidationService {
  validateAssignmentRules(assigned: AssignedTaskVm[]): boolean {
    const total = assigned.length;
    if (total < 5 || total > 11) {
      window.alert('Liczba przypisanych zadań musi zawierać się w przedziale 5–11.');
      return false;
    }

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

  isAssignmentAllowed(task: AvailableTaskVm, assigned: AssignedTaskVm[], user: ListAllUsersItem): boolean {
    const updated = [...assigned, task];

    if (updated.length > 11) {
      window.alert('Nie można przypisać więcej niż 11 zadań.');
      return false;
    }

    const isDev = user.role === 'Developer';
    const isOps = user.role === 'DevOps' || user.role === 'Administrator';

    if (isDev && task.type !== 'Implementation') {
      window.alert('Programista może mieć przypisane tylko zadania typu Implementacja.');
      return false;
    }

    if (!isDev && !isOps) {
      window.alert(`Rola użytkownika "${user.role}" nie ma uprawnień do przypisywania zadań.`);
      return false;
    }

    return true;
  }
}
