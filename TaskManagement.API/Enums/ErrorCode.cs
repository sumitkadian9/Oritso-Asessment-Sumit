namespace TaskManagement.API.Enums;

public enum ErrorCode
{
    // Account related errors
    NotAuthorized = 1,
    AccessDenied = 2,
    DuplicateUser = 3,
    RegistrationFailed = 4,
    UserNotFound = 5,
    UserCreationFailed = 6,
    UserDeletionFailed = 7,
    UserNotDeletable = 8,
    UserUpdateFailed = 9,
    PasswordUpdateFailed = 10,
    InvalidCredentials = 11,
    TodoTaskNotFound = 12,
    TodoTaskUpdateFailed = 13,
    TodoTaskDeletionFailed = 14,
    TodoTaskCreationFailed = 15
}