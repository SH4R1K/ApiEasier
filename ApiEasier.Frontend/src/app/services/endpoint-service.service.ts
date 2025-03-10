import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Endpoint } from '../interfaces/Endpoint';

@Injectable({
  providedIn: 'root',
})
export class EndpointService {
  private baseUrl = `${window.location.origin}/api`;

  constructor(private http: HttpClient) {}

  getEndpointList(
    apiServiceName: string,
    entityName: string
  ): Observable<Endpoint[]> {
    return this.http.get<Endpoint[]>(
      `${this.baseUrl}/ApiEndpoint/${apiServiceName}/${entityName}`
    );
  }

  createEndpoint(
    apiServiceName: string,
    entityName: string,
    action: Endpoint
  ): Observable<Endpoint> {
    return this.http.post<Endpoint>(
      `${this.baseUrl}/ApiEndpoint/${apiServiceName}/${entityName}`,
      action
    );
  }

  getEndpointByName(
    apiServiceName: string,
    entityName: string,
    actionName: string
  ): Observable<Endpoint> {
    return this.http.get<Endpoint>(
      `${this.baseUrl}/ApiEndpoint/${apiServiceName}/${entityName}/${actionName}`
    );
  }

  updateEndpoint(
    apiServiceName: string,
    entityName: string,
    actionName: string,
    action: Endpoint
  ): Observable<Endpoint> {
    return this.http.put<Endpoint>(
      `${this.baseUrl}/ApiEndpoint/${apiServiceName}/${entityName}/${actionName}`,
      action
    );
  }

  deleteEndpoint(
    apiServiceName: string,
    entityName: string,
    actionName: string
  ): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/ApiEndpoint/${apiServiceName}/${entityName}/${actionName}`
    );
  }

  updateEndpointStatus(
    serviceName: string,
    entityName: string,
    endpoint: string,
    isActive: boolean
  ): Observable<any> {
    return this.http.patch<any>(
      `${this.baseUrl}/ApiEndpoint/${serviceName}/${entityName}/${endpoint}/${isActive}`,
      null
    );
  }
}
