import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { Endpoint } from "../interfaces/Endpoint";
import { TuiAlertService } from '@taiga-ui/core';
import { ErrorHandlerService } from './error-handler.service';

@Injectable({
  providedIn: 'root',
})
export class EndpointService {
  private baseUrl = `${window.location.origin}/api`;

  constructor(
    private http: HttpClient,
    private errorHandler: ErrorHandlerService,
    private alerts: TuiAlertService
  ) {}

  getEndpointList(
    apiServiceName: string,
    entityName: string
  ): Observable<Endpoint[]> {
    return this.http
      .get<Endpoint[]>(
        `${this.baseUrl}/ApiEndpoint/${apiServiceName}/${entityName}`
      )
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  createEndpoint(
    apiServiceName: string,
    entityName: string,
    action: Endpoint
  ): Observable<Endpoint> {
    return this.http
      .post<Endpoint>(
        `${this.baseUrl}/ApiEndpoint/${apiServiceName}/${entityName}`,
        action
      )
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  getEndpointByName(
    apiServiceName: string,
    entityName: string,
    actionName: string
  ): Observable<Endpoint> {
    return this.http
      .get<Endpoint>(
        `${this.baseUrl}/ApiEndpoint/${apiServiceName}/${entityName}/${actionName}`
      )
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  updateEndpoint(
    apiServiceName: string,
    entityName: string,
    actionName: string,
    action: Endpoint
  ): Observable<Endpoint> {
    return this.http
      .put<Endpoint>(
        `${this.baseUrl}/ApiEndpoint/${apiServiceName}/${entityName}/${actionName}`,
        action
      )
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  deleteEndpoint(
    apiServiceName: string,
    entityName: string,
    actionName: string
  ): Observable<void> {
    return this.http
      .delete<void>(
        `${this.baseUrl}/ApiEndpoint/${apiServiceName}/${entityName}/${actionName}`
      )
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  updateEndpointStatus(
    serviceName: string,
    entityName: string,
    endpoint: string,
    isActive: boolean
  ): Observable<any> {
    return this.http
      .patch<any>(
        `${this.baseUrl}/ApiEndpoint/${serviceName}/${entityName}/${endpoint}/${isActive}`,
        null
      )
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    this.errorHandler.handleError(error);
    this.alerts.open(error.message, { appearance: 'negative' }).subscribe();
    return throwError(error);
  }
}
