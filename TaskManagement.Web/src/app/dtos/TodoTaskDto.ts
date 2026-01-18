import { TaskCompletionStatus } from "../enums/TaskCompletionStatus";
import { UserMinDto } from "./UserDto";

export interface TodoTaskDto {
    id?: string;
    title: string;
    description: string;
    dueDate: string; // HTML date inputs and .NET DateOnly use "YYYY-MM-DD"
    status: TaskCompletionStatus;
    remarks: string;
    user?: UserMinDto;
    createdOn?: number;
    lastUpdatedOn?: number;
}