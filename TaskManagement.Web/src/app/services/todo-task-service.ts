import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { TodoTaskDto } from '../dtos/TodoTaskDto';

@Injectable({
  providedIn: 'root',
})

export class TodoTaskService {
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/TodoTasks`;

  getTodoTasks(searchParam?: string, pageNumber: number = 1, pageSize: number = 10): Observable<TodoTaskDto[]> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (searchParam) {
      params = params.set('searchParam', searchParam);
    }

    return this.http.get<TodoTaskDto[]>(this.url, { params });
  }

  createTodoTask(task: TodoTaskDto): Observable<TodoTaskDto> {
    return this.http.post<TodoTaskDto>(this.url, task);
  }

  updateTodoTask(task: TodoTaskDto): Observable<number> {
    return this.http.put<number>(this.url, task);
  }

  deleteTodoTask(id: string): Observable<number> {
    return this.http.delete<number>(`${this.url}/${id}`);
  }
}
