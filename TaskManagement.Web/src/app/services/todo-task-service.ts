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
}
