import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiServiceStructure } from '../interfaces/ApiServiceStructure';
import { apiServiceShortStructure } from '../interfaces/apiServiceShortStructure';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private baseUrl = `${window.location.origin}/api`;

  constructor(private http: HttpClient) {}

  getApiList(): Observable<apiServiceShortStructure[]> {
    return this.http.get<apiServiceShortStructure[]>(
      `${this.baseUrl}/ApiService`
    );
  }

  getApiStructureList(name: string): Observable<ApiServiceStructure> {
    return this.http.get<ApiServiceStructure>(
      `${this.baseUrl}/ApiService/${encodeURIComponent(name)}`
    );
  }

  createApiService(
    service: apiServiceShortStructure
  ): Observable<apiServiceShortStructure> {
    return this.http.post<apiServiceShortStructure>(
      `${this.baseUrl}/ApiService`,
      service
    );
  }

  createFullApiService(service: ApiServiceStructure): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/ApiService`, service);
  }

  updateApiService(
    oldName: string,
    service: apiServiceShortStructure
  ): Observable<apiServiceShortStructure> {
    return this.http.put<apiServiceShortStructure>(
      `${this.baseUrl}/ApiService/${encodeURIComponent(oldName)}`,
      service
    );
  }

  deleteApiService(serviceName: string): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/ApiService/${encodeURIComponent(serviceName)}`
    );
  }

  updateApiServiceStatus(
    serviceName: string,
    isActive: boolean
  ): Observable<any> {
    return this.http.patch<any>(
      `${this.baseUrl}/ApiService/${encodeURIComponent(
        serviceName
      )}/${isActive}`,
      null
    );
  }
}
