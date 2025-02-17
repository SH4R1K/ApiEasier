import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { TuiAlertService } from '@taiga-ui/core';
import {
  apiServiceShortStructure,
  ApiServiceStructure,
} from './service-structure-api';
import { ErrorHandlerService } from './error-handler.service';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private baseUrl = `${window.location.origin}/api`;

  constructor(
    private http: HttpClient,
    private errorHandler: ErrorHandlerService,
    private alerts: TuiAlertService
  ) {}

  getApiList(): Observable<apiServiceShortStructure[]> {
    return this.http
      .get<apiServiceShortStructure[]>(`${this.baseUrl}/ApiService`)
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  getApiStructureList(name: string): Observable<ApiServiceStructure> {
    return this.http
      .get<ApiServiceStructure>(
        `${this.baseUrl}/ApiService/${encodeURIComponent(name)}`
      )
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  createApiService(
    service: apiServiceShortStructure
  ): Observable<apiServiceShortStructure> {
    return this.http
      .post<apiServiceShortStructure>(`${this.baseUrl}/ApiService`, service)
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  createFullApiService(service: ApiServiceStructure): Observable<void> {
    return this.http
      .post<void>(`${this.baseUrl}/ApiService`, service)
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  updateApiService(
    oldName: string,
    service: apiServiceShortStructure
  ): Observable<apiServiceShortStructure> {
    return this.http
      .put<apiServiceShortStructure>(
        `${this.baseUrl}/ApiService/${encodeURIComponent(oldName)}`,
        service
      )
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  deleteApiService(serviceName: string): Observable<void> {
    return this.http
      .delete<void>(
        `${this.baseUrl}/ApiService/${encodeURIComponent(serviceName)}`
      )
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  updateApiServiceStatus(
    serviceName: string,
    isActive: boolean
  ): Observable<any> {
    return this.http
      .patch<any>(
        `${this.baseUrl}/ApiService/${encodeURIComponent(
          serviceName
        )}/${isActive}`,
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
