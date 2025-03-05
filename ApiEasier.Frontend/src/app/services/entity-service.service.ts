import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Entity } from '../interfaces/Entity';
import { apiServiceShortStructure } from '../interfaces/apiServiceShortStructure';

@Injectable({
  providedIn: 'root',
})
export class EntityService {
  private baseUrl = `${window.location.origin}/api`;

  constructor(private http: HttpClient) {}

  getApiEntityList(apiServiceName: string): Observable<Entity[]> {
    return this.http.get<Entity[]>(
      `${this.baseUrl}/ApiEntity/${apiServiceName}`
    );
  }

  getApiEntity(apiServiceName: string, entityName: string): Observable<Entity> {
    return this.http.get<Entity>(
      `${this.baseUrl}/ApiEntity/${apiServiceName}/${entityName}`
    );
  }

  createApiEntity(apiServiceName: string, entity: Entity): Observable<Entity> {
    return this.http.post<Entity>(
      `${this.baseUrl}/ApiEntity/${apiServiceName}`,
      entity
    );
  }

  updateApiEntity(
    apiServiceName: string,
    entityName: string,
    entity: Entity
  ): Observable<Entity> {
    return this.http.put<Entity>(
      `${this.baseUrl}/ApiEntity/${apiServiceName}/${entityName}`,
      entity
    );
  }

  deleteApiEntity(
    apiServiceName: string,
    entityName: string
  ): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/ApiEntity/${apiServiceName}/${entityName}`
    );
  }

  updateEntityStatus(
    serviceName: string,
    entityName: string,
    isActive: boolean
  ): Observable<any> {
    return this.http.patch<any>(
      `${this.baseUrl}/ApiEntity/${serviceName}/${entityName}/${isActive}`,
      null
    );
  }

  getAllApiServices(): Observable<apiServiceShortStructure[]> {
    return this.http.get<apiServiceShortStructure[]>(
      `${this.baseUrl}/ApiServices`
    );
  }
}
