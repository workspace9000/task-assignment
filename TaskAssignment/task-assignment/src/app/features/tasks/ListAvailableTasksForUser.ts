import { ListAvailableTasksForUserItem } from "./list-available-tasks-for-user-item";

export interface ListAvailableTasksForUser {
    items: ListAvailableTasksForUserItem[];
    totalPages: number;
}