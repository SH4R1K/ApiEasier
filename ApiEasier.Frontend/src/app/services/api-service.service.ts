import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { TuiAlertService } from '@taiga-ui/core';
import { ApiServiceStructure } from "../interfaces/ApiServiceStructure";
import { apiServiceShortStructure } from "../interfaces/apiServiceShortStructure";
import { ErrorHandlerService } from './error-handler.service';

/**
 * Сервис для взаимодействия с API сервисами.
 *
 * @remarks
 * Этот сервис предоставляет методы для получения, создания, обновления и удаления API сервисов.
 * Он использует HttpClient для выполнения HTTP-запросов и обрабатывает ошибки с помощью ErrorHandlerService.
 *
 * @type {ApiService}
 * @memberof Component
 */
@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private baseUrl = `${window.location.origin}/api`;

  constructor(private http: HttpClient) {}

  /**
   * Получает список всех API сервисов.
   *
   * @returns {Observable<apiServiceShortStructure[]>} Наблюдаемый объект, содержащий список API сервисов.
   * @memberof ApiService
   */
  getApiList(): Observable<apiServiceShortStructure[]> {
    return this.http.get<apiServiceShortStructure[]>(
      `${this.baseUrl}/ApiService`
    );
  }

  /**
   * Получает структуру API сервиса по его имени.
   *
   * @param {string} name - Имя API сервиса.
   * @returns {Observable<ApiServiceStructure>} Наблюдаемый объект, содержащий структуру API сервиса.
   * @memberof ApiService
   */
  getApiStructureList(name: string): Observable<ApiServiceStructure> {
    return this.http.get<ApiServiceStructure>(
      `${this.baseUrl}/ApiService/${encodeURIComponent(name)}`
    );
  }

  /**
   * Создает новый API сервис.
   *
   * @param {apiServiceShortStructure} service - Краткая структура API сервиса.
   * @returns {Observable<apiServiceShortStructure>} Наблюдаемый объект, содержащий созданный API сервис.
   * @memberof ApiService
   */
  createApiService(
    service: apiServiceShortStructure
  ): Observable<apiServiceShortStructure> {
    return this.http.post<apiServiceShortStructure>(
      `${this.baseUrl}/ApiService`,
      service
    );
  }

  /**
   * Создает полный API сервис.
   *
   * @param {ApiServiceStructure} service - Полная структура API сервиса.
   * @returns {Observable<void>} Наблюдаемый объект, указывающий на успешное создание.
   * @memberof ApiService
   */
  createFullApiService(service: ApiServiceStructure): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/ApiService`, service);
  }

  /**
   * Обновляет существующий API сервис.
   *
   * @param {string} oldName - Старое имя API сервиса.
   * @param {apiServiceShortStructure} service - Новая структура API сервиса.
   * @returns {Observable<apiServiceShortStructure>} Наблюдаемый объект, содержащий обновленный API сервис.
   * @memberof ApiService
   */
  updateApiService(
    oldName: string,
    service: apiServiceShortStructure
  ): Observable<apiServiceShortStructure> {
    return this.http.put<apiServiceShortStructure>(
      `${this.baseUrl}/ApiService/${encodeURIComponent(oldName)}`,
      service
    );
  }

  /**
   * Удаляет API сервис по его имени.
   *
   * @param {string} serviceName - Имя API сервиса.
   * @returns {Observable<void>} Наблюдаемый объект, указывающий на успешное удаление.
   * @memberof ApiService
   */
  deleteApiService(serviceName: string): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/ApiService/${encodeURIComponent(serviceName)}`
    );
  }

  /**
   * Обновляет статус активности API сервиса.
   *
   * @param {string} serviceName - Имя API сервиса.
   * @param {boolean} isActive - Новый статус активности.
   * @returns {Observable<any>} Наблюдаемый объект, указывающий на успешное обновление статуса.
   * @memberof ApiService
   */
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
