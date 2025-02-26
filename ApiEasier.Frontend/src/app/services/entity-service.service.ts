import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { Entity } from "../interfaces/Entity";
import { ApiServiceStructure } from "../interfaces/ApiServiceStructure";
import { apiServiceShortStructure } from "../interfaces/apiServiceShortStructure";
import { TuiAlertService } from '@taiga-ui/core';
import { ErrorHandlerService } from './error-handler.service';

@Injectable({
  providedIn: 'root',
})
export class EntityService {
  private baseUrl = `${window.location.origin}/api`;

  constructor(
    private http: HttpClient,
    private errorHandler: ErrorHandlerService,
    private alerts: TuiAlertService
  ) {}

  getApiEntityList(apiServiceName: string): Observable<Entity[]> {
    return this.http
      .get<Entity[]>(`${this.baseUrl}/ApiEntity/${apiServiceName}`)
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  getApiEntity(apiServiceName: string, entityName: string): Observable<Entity> {
    return this.http
      .get<Entity>(`${this.baseUrl}/ApiEntity/${apiServiceName}/${entityName}`)
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  createApiEntity(apiServiceName: string, entity: Entity): Observable<Entity> {
    return this.http
      .post<Entity>(`${this.baseUrl}/ApiEntity/${apiServiceName}`, entity)
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  updateApiEntity(
    apiServiceName: string,
    entityName: string,
    entity: Entity
  ): Observable<Entity> {
    return this.http
      .put<Entity>(
        `${this.baseUrl}/ApiEntity/${apiServiceName}/${entityName}`,
        entity
      )
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  deleteApiEntity(
    apiServiceName: string,
    entityName: string
  ): Observable<void> {
    return this.http
      .delete<void>(`${this.baseUrl}/ApiEntity/${apiServiceName}/${entityName}`)
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  updateEntityStatus(
    serviceName: string,
    entityName: string,
    isActive: boolean
  ): Observable<any> {
    return this.http
      .patch<any>(
        `${this.baseUrl}/ApiEntity/${serviceName}/${entityName}/${isActive}`,
        null
      )
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  getAllApiServices(): Observable<apiServiceShortStructure[]> {
    return this.http
      .get<apiServiceShortStructure[]>(`${this.baseUrl}/ApiServices`)
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    this.errorHandler.handleError(error);
    this.alerts.open(error.message, { appearance: 'negative' }).subscribe();
    return throwError(error);
  }
}
