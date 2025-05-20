import { ListAvailableTasksForUserItem } from '../tasks/list-available-tasks-for-user-item';

export interface AvailableTaskVm extends ListAvailableTasksForUserItem {
    isDisabled?: boolean;
}
