import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiServiceStructure } from "../interfaces/ApiServiceStructure";
import { apiServiceShortStructure } from "../interfaces/apiServiceShortStructure";
import { ApiService } from '../services/api-service.service';

/**
 * Сервис для управления API сервисами.
 *
 * @remarks
 * Этот сервис предоставляет методы для получения, создания, обновления и удаления API сервисов.
 * Он взаимодействует с `ApiService` для выполнения операций.
 *
 * @type {ApiServiceRepositoryService}
 * @memberof Component
 */
@Injectable({
  providedIn: 'root',
})
export class ApiServiceRepositoryService {
  constructor(private apiService: ApiService) {}

  /**
   * Получает список всех API сервисов.
   *
   * @returns {Observable<apiServiceShortStructure[]>} Наблюдаемый объект, содержащий список API сервисов.
   * @memberof ApiServiceRepositoryService
   */
  getApiList(): Observable<apiServiceShortStructure[]> {
    return this.apiService.getApiList();
  }

  /**
   * Получает структуру API сервиса по его имени.
   *
   * @param {string} name - Имя API сервиса.
   * @returns {Observable<ApiServiceStructure>} Наблюдаемый объект, содержащий структуру API сервиса.
   * @memberof ApiServiceRepositoryService
   */
  getApiStructureList(name: string): Observable<ApiServiceStructure> {
    return this.apiService.getApiStructureList(name);
  }

  /**
   * Создает новый API сервис.
   *
   * @param {apiServiceShortStructure} service - Краткая структура API сервиса.
   * @returns {Observable<apiServiceShortStructure>} Наблюдаемый объект, содержащий созданный API сервис.
   * @memberof ApiServiceRepositoryService
   */
  createApiService(
    service: apiServiceShortStructure
  ): Observable<apiServiceShortStructure> {
    return this.apiService.createApiService(service);
  }

  /**
   * Создает полный API сервис.
   *
   * @param {ApiServiceStructure} service - Полная структура API сервиса.
   * @returns {Observable<void>} Наблюдаемый объект, указывающий на успешное создание.
   * @memberof ApiServiceRepositoryService
   */
  createFullApiService(service: ApiServiceStructure): Observable<void> {
    return this.apiService.createFullApiService(service);
  }

  /**
   * Обновляет существующий API сервис.
   *
   * @param {string} oldName - Старое имя API сервиса.
   * @param {apiServiceShortStructure} service - Новая структура API сервиса.
   * @returns {Observable<apiServiceShortStructure>} Наблюдаемый объект, содержащий обновленный API сервис.
   * @memberof ApiServiceRepositoryService
   */
  updateApiService(
    oldName: string,
    service: apiServiceShortStructure
  ): Observable<apiServiceShortStructure> {
    return this.apiService.updateApiService(oldName, service);
  }

  /**
   * Удаляет API сервис по его имени.
   *
   * @param {string} serviceName - Имя API сервиса.
   * @returns {Observable<void>} Наблюдаемый объект, указывающий на успешное удаление.
   * @memberof ApiServiceRepositoryService
   */
  deleteApiService(serviceName: string): Observable<void> {
    return this.apiService.deleteApiService(serviceName);
  }

  /**
   * Обновляет статус активности API сервиса.
   *
   * @param {string} serviceName - Имя API сервиса.
   * @param {boolean} isActive - Новый статус активности.
   * @returns {Observable<any>} Наблюдаемый объект, указывающий на успешное обновление статуса.
   * @memberof ApiServiceRepositoryService
   */
  updateApiServiceStatus(
    serviceName: string,
    isActive: boolean
  ): Observable<any> {
    return this.apiService.updateApiServiceStatus(serviceName, isActive);
  }
}
