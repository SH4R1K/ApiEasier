import { Injectable } from '@angular/core';
import { EndpointService as EndpointService } from '../services/endpoint-service.service';
import { Endpoint as Endpoint } from "../interfaces/Endpoint";
import { Observable } from 'rxjs';

/**
 * Сервис для управления конечными точками (endpoints).
 *
 * @remarks
 * Этот сервис предоставляет методы для получения, создания, обновления и удаления конечных точек.
 * Он взаимодействует с `EndpointService` для выполнения операций.
 *
 * @type {EndpointRepositoryService}
 * @memberof Component
 */
@Injectable({
  providedIn: 'root'
})
export class EndpointRepositoryService {

  constructor(private endpointService: EndpointService) { }

  /**
   * Получает список конечных точек для указанного API сервиса и сущности.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @returns {Observable<Endpoint[]>} Наблюдаемый объект, содержащий список конечных точек.
   * @memberof EndpointRepositoryService
   */
  getEndpointList(apiServiceName: string, entityName: string): Observable<Endpoint[]> {
    return this.endpointService.getEndpointList(apiServiceName, entityName);
  }

  /**
   * Создает новую конечную точку для указанного API сервиса и сущности.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @param {Endpoint} endpoint - Структура новой конечной точки.
   * @returns {Observable<Endpoint>} Наблюдаемый объект, содержащий созданную конечную точку.
   * @memberof EndpointRepositoryService
   */
  createEndpoint(apiServiceName: string, entityName: string, endpoint: Endpoint): Observable<Endpoint> {
    return this.endpointService.createEndpoint(apiServiceName, entityName, endpoint);
  }

  /**
   * Получает конечную точку по её имени для указанного API сервиса и сущности.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @param {string} endpointName - Имя конечной точки.
   * @returns {Observable<Endpoint>} Наблюдаемый объект, содержащий конечную точку.
   * @memberof EndpointRepositoryService
   */
  getEndpointByName(apiServiceName: string, entityName: string, endpointName: string): Observable<Endpoint> {
    return this.endpointService.getEndpointByName(apiServiceName, entityName, endpointName);
  }

  /**
   * Обновляет существующую конечную точку для указанного API сервиса и сущности.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @param {string} endpointName - Имя конечной точки.
   * @param {Endpoint} endpoint - Новая структура конечной точки.
   * @returns {Observable<Endpoint>} Наблюдаемый объект, содержащий обновленную конечную точку.
   * @memberof EndpointRepositoryService
   */
  updateEndpoint(apiServiceName: string, entityName: string, endpointName: string, endpoint: Endpoint): Observable<Endpoint> {
    return this.endpointService.updateEndpoint(apiServiceName, entityName, endpointName, endpoint);
  }

  /**
   * Удаляет конечную точку по её имени для указанного API сервиса и сущности.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @param {string} endpointName - Имя конечной точки.
   * @returns {Observable<void>} Наблюдаемый объект, указывающий на успешное удаление.
   * @memberof EndpointRepositoryService
   */
  deleteEndpoint(apiServiceName: string, entityName: string, endpointName: string): Observable<void> {
    return this.endpointService.deleteEndpoint(apiServiceName, entityName, endpointName);
  }

  /**
   * Обновляет статус активности конечной точки.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @param {string} endpointName - Имя конечной точки.
   * @param {boolean} isActive - Новый статус активности.
   * @returns {Observable<any>} Наблюдаемый объект, указывающий на успешное обновление статуса.
   * @memberof EndpointRepositoryService
   */
  updateEndpointStatus(apiServiceName: string, entityName: string, endpointName: string, isActive: boolean): Observable<any> {
    return this.endpointService.updateEndpointStatus(apiServiceName, entityName, endpointName, isActive);
  }
}
