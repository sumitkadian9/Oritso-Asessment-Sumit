import { Component, inject, OnInit } from '@angular/core';
import { TodoTaskDto } from '../../dtos/TodoTaskDto';
import { TaskCompletionStatus } from '../../enums/TaskCompletionStatus';
import { TodoTaskService } from '../../services/todo-task-service';
import { AccountService } from '../../services/account-service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
  standalone: true
})
export class Dashboard implements OnInit {
  private todoService = inject(TodoTaskService);
  private accountService = inject(AccountService);
  private router = inject(Router);

  tasks: TodoTaskDto[] = [];
  statusEnum = TaskCompletionStatus;

  private searchSubject = new Subject<string>();
  searchQuery: string = '';
  pageNumber: number = 1;
  pageSize: number = 20;

  ngOnInit(): void {
    this.searchSubject.pipe(
      debounceTime(400),  
      distinctUntilChanged()
    ).subscribe(term => {
      this.searchQuery = term;
      this.pageNumber = 1;
      this.loadTasks();
    });
    this.loadTasks();
  }

  onSearch(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.searchSubject.next(value);
  }

  loadTasks() {
    this.todoService.getTodoTasks().subscribe({
      next: (data) => this.tasks = data,
      error: (err) => console.error('Error fetching tasks', err)
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigate(['/login']);
  }

  formatDate(unixTimestamp: number | undefined): string {
    if (!unixTimestamp) return 'N/A';
    return new Date(unixTimestamp * 1000).toLocaleDateString();
  }

  nextPage() {
    this.pageNumber++;
    this.loadTasks();
  }

  prevPage() {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadTasks();
    }
  }
}
