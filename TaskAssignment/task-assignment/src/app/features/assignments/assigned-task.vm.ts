import { ListAssignedTasksForUserItem } from "./list-assigned-tasks-for-user-item";

export interface AssignedTaskVm extends ListAssignedTasksForUserItem {
    isNew?: boolean;
    isInvalid?: boolean;
}
