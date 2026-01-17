using FluentResults;
using TaskManagement.API.Enums;

namespace TaskManagement.API.Errors;

public static class CustomErrors
{
    public static class Account
    {
        public static Error DuplicateUser => new Error("User already exists")
            .WithMetadata("ErrorCode", ErrorCode.DuplicateUser);

        public static Error RegistrationFailed => new Error("Failed to register user")
            .WithMetadata("ErrorCode", ErrorCode.RegistrationFailed);

        public static Error UserNotFound => new Error("User not found")
            .WithMetadata("ErrorCode", ErrorCode.UserNotFound);

        public static Error UserNotDeletable => new Error("User not deletable")
            .WithMetadata("ErrorCode", ErrorCode.UserNotDeletable); 

        public static Error UserUpdateFailed => new Error("Failed to update user")
            .WithMetadata("ErrorCode", ErrorCode.UserUpdateFailed);
        
        public static Error PasswordUpdateFailed => new Error("Failed to update user password")
            .WithMetadata("ErrorCode", ErrorCode.PasswordUpdateFailed);

        public static Error UserDeletionFailed => new Error("Failed to delete user")
            .WithMetadata("ErrorCode", ErrorCode.UserDeletionFailed);

        public static Error UserCreationFailed => new Error("Failed to create user")
            .WithMetadata("ErrorCode", ErrorCode.UserCreationFailed);   

        public static Error InvalidCredentials => new Error("Invalid credentials")
            .WithMetadata("ErrorCode", ErrorCode.InvalidCredentials);
    }

    public static class TodoTask
    {
        public static Error TodoTaskNotFound => new Error("Task not found")
            .WithMetadata("ErrorCode", ErrorCode.TodoTaskNotFound);
        
        public static Error TodoTaskUpdateFailed => new Error("Failed to update task")
            .WithMetadata("ErrorCode", ErrorCode.TodoTaskUpdateFailed);
        
        public static Error TodoTaskDeletionFailed => new Error("Failed to delete task")
            .WithMetadata("ErrorCode", ErrorCode.TodoTaskDeletionFailed);
        
        public static Error TodoTaskCreationFailed => new Error("Failed to create task")
            .WithMetadata("ErrorCode", ErrorCode.TodoTaskCreationFailed);
    }
}