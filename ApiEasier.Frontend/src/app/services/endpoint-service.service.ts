import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoint } from '../interfaces/Endpoint';
import { HttpClient } from '@angular/common/http';
/**
 * Сервис для управления конечными точками (endpoints).
 *
 * @remarks
 * Этот сервис предоставляет методы для получения, создания, обновления и удаления конечных точек.
 * Он использует HttpClient для выполнения HTTP-запросов и обрабатывает ошибки с помощью ErrorHandlerService.
 *
 * @type {EndpointService}
 * @memberof Component
 */
@Injectable({
  providedIn: 'root',
})
export class EndpointService {
  private baseUrl = `${window.location.origin}/api`;

  constructor(
    private http: HttpClient,
  ) {}

  /**
   * Получает список конечных точек для указанного API сервиса и сущности.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @returns {Observable<Endpoint[]>} Наблюдаемый объект, содержащий список конечных точек.
   * @memberof EndpointService
   */
  getEndpointList(
    apiServiceName: string,
    entityName: string
  ): Observable<Endpoint[]> {
    return this.http.get<Endpoint[]>(
      `${this.baseUrl}/ApiEndpoint/${apiServiceName}/${entityName}`
    );
  }

  /**
   * Создает новую конечную точку для указанного API сервиса и сущности.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @param {Endpoint} action - Структура новой конечной точки.
   * @returns {Observable<Endpoint>} Наблюдаемый объект, содержащий созданную конечную точку.
   * @memberof EndpointService
   */
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

  /**
   * Получает конечную точку по её имени для указанного API сервиса и сущности.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @param {string} actionName - Имя конечной точки.
   * @returns {Observable<Endpoint>} Наблюдаемый объект, содержащий конечную точку.
   * @memberof EndpointService
   */
  getEndpointByName(
    apiServiceName: string,
    entityName: string,
    actionName: string
  ): Observable<Endpoint> {
    return this.http.get<Endpoint>(
      `${this.baseUrl}/ApiEndpoint/${apiServiceName}/${entityName}/${actionName}`
    );
  }

  /**
   * Обновляет существующую конечную точку для указанного API сервиса и сущности.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @param {string} actionName - Имя конечной точки.
   * @param {Endpoint} action - Новая структура конечной точки.
   * @returns {Observable<Endpoint>} Наблюдаемый объект, содержащий обновленную конечную точку.
   * @memberof EndpointService
   */
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

  /**
   * Удаляет конечную точку по её имени для указанного API сервиса и сущности.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @param {string} actionName - Имя конечной точки.
   * @returns {Observable<void>} Наблюдаемый объект, указывающий на успешное удаление.
   * @memberof EndpointService
   */
  deleteEndpoint(
    apiServiceName: string,
    entityName: string,
    actionName: string
  ): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/ApiEndpoint/${apiServiceName}/${entityName}/${actionName}`
    );
  }

  /**
   * Обновляет статус активности конечной точки.
   *
   * @param {string} serviceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @param {string} endpoint - Имя конечной точки.
   * @param {boolean} isActive - Новый статус активности.
   * @returns {Observable<any>} Наблюдаемый объект, указывающий на успешное обновление статуса.
   * @memberof EndpointService
   */
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

