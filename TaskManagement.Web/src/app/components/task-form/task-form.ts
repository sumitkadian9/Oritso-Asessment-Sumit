import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TodoTaskDto } from '../../dtos/TodoTaskDto';
import { TaskCompletionStatus } from '../../enums/TaskCompletionStatus';

@Component({
  selector: 'app-task-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-form.html',
  styleUrl: './task-form.css',
})
export class TaskForm implements OnInit {
  private fb = inject(FormBuilder);
  
  @Input() taskToEdit: TodoTaskDto | null = null;
  @Output() close = new EventEmitter<void>();
  @Output() saved = new EventEmitter<TodoTaskDto>();

  taskForm: FormGroup = this.fb.group({
    id: [null],
    title: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.required]],
    dueDate: ['', [Validators.required]],
    status: [TaskCompletionStatus.Pending, [Validators.required]],
    remarks: ['']
  });

  statusOptions = [
    { label: 'Pending', value: TaskCompletionStatus.Pending },
    { label: 'In Progress', value: TaskCompletionStatus.InProgress },
    { label: 'Completed', value: TaskCompletionStatus.Completed }
  ];

  ngOnInit() {
    if (this.taskToEdit) {
      this.taskForm.patchValue(this.taskToEdit);
    }
  }

  submit() {
    if (this.taskForm.valid) {
      const formValue = { ...this.taskForm.value };
      
      // Force status to be number to avoid enum issues
      formValue.status = +formValue.status; 
      this.saved.emit(formValue);
    }
  }
}