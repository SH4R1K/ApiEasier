import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Entity } from "../interfaces/Entity";
import { apiServiceShortStructure } from "../interfaces/apiServiceShortStructure";
import { TuiAlertService } from '@taiga-ui/core';
import { ErrorHandlerService } from './error-handler.service';

/**
 * Сервис для управления сущностями API.
 *
 * @remarks
 * Этот сервис предоставляет методы для получения, создания, обновления и удаления сущностей API.
 * Он использует HttpClient для выполнения HTTP-запросов и обрабатывает ошибки с помощью ErrorHandlerService.
 *
 * @type {EntityService}
 * @memberof Component
 */
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

  /**
   * Получает список сущностей для указанного API сервиса.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @returns {Observable<Entity[]>} Наблюдаемый объект, содержащий список сущностей.
   * @memberof EntityService
   */
  getApiEntityList(apiServiceName: string): Observable<Entity[]> {
    return this.http
      .get<Entity[]>(`${this.baseUrl}/ApiEntity/${apiServiceName}`)
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  /**
   * Получает сущность по её имени для указанного API сервиса.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @returns {Observable<Entity>} Наблюдаемый объект, содержащий сущность.
   * @memberof EntityService
   */
  getApiEntity(apiServiceName: string, entityName: string): Observable<Entity> {
    return this.http
      .get<Entity>(`${this.baseUrl}/ApiEntity/${apiServiceName}/${entityName}`)
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  /**
   * Создает новую сущность для указанного API сервиса.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {Entity} entity - Структура новой сущности.
   * @returns {Observable<Entity>} Наблюдаемый объект, содержащий созданную сущность.
   * @memberof EntityService
   */
  createApiEntity(apiServiceName: string, entity: Entity): Observable<Entity> {
    return this.http
      .post<Entity>(`${this.baseUrl}/ApiEntity/${apiServiceName}`, entity)
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  /**
   * Обновляет существующую сущность для указанного API сервиса.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @param {Entity} entity - Новая структура сущности.
   * @returns {Observable<Entity>} Наблюдаемый объект, содержащий обновленную сущность.
   * @memberof EntityService
   */
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

  /**
   * Удаляет сущность по её имени для указанного API сервиса.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @returns {Observable<void>} Наблюдаемый объект, указывающий на успешное удаление.
   * @memberof EntityService
   */
  deleteApiEntity(
    apiServiceName: string,
    entityName: string
  ): Observable<void> {
    return this.http
      .delete<void>(`${this.baseUrl}/ApiEntity/${apiServiceName}/${entityName}`)
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  /**
   * Обновляет статус активности сущности.
   *
   * @param {string} serviceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @param {boolean} isActive - Новый статус активности.
   * @returns {Observable<any>} Наблюдаемый объект, указывающий на успешное обновление статуса.
   * @memberof EntityService
   */
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

  /**
   * Получает список всех API сервисов.
   *
   * @returns {Observable<apiServiceShortStructure[]>} Наблюдаемый объект, содержащий список API сервисов.
   * @memberof EntityService
   */
  getAllApiServices(): Observable<apiServiceShortStructure[]> {
    return this.http
      .get<apiServiceShortStructure[]>(`${this.baseUrl}/ApiServices`)
      .pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
  }

  /**
   * Обрабатывает ошибки HTTP-запросов.
   *
   * @private
   * @param {HttpErrorResponse} error - Объект ошибки HTTP.
   * @returns {Observable<never>} Наблюдаемый объект, указывающий на ошибку.
   * @memberof EntityService
   */
  private handleError(error: HttpErrorResponse): Observable<never> {
    this.errorHandler.handleError(error);
    this.alerts.open(error.message, { appearance: 'negative' }).subscribe();
    return throwError(error);
  }
}
