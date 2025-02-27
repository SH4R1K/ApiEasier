import { Injectable } from '@angular/core';
import { EntityService } from '../services/entity-service.service';
import { Observable } from 'rxjs';
import { Entity } from "../interfaces/Entity";

/**
 * Сервис для управления сущностями.
 *
 * @remarks
 * Этот сервис предоставляет методы для получения, создания, обновления и удаления сущностей.
 * Он взаимодействует с `EntityService` для выполнения операций.
 *
 * @type {EntityRepositoryService}
 * @memberof Component
 */
@Injectable({
  providedIn: 'root'
})
export class EntityRepositoryService {

  constructor(private entityService: EntityService) { }

  /**
   * Получает список сущностей для указанного API сервиса.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @returns {Observable<Entity[]>} Наблюдаемый объект, содержащий список сущностей.
   * @memberof EntityRepositoryService
   */
  getApiEntityList(apiServiceName: string): Observable<Entity[]> {
    return this.entityService.getApiEntityList(apiServiceName);
  }

  /**
   * Получает сущность по её имени для указанного API сервиса.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @returns {Observable<Entity>} Наблюдаемый объект, содержащий сущность.
   * @memberof EntityRepositoryService
   */
  getApiEntity(apiServiceName: string, entityName: string): Observable<Entity> {
    return this.entityService.getApiEntity(apiServiceName, entityName);
  }

  /**
   * Создает новую сущность для указанного API сервиса.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {Entity} entity - Структура новой сущности.
   * @returns {Observable<Entity>} Наблюдаемый объект, содержащий созданную сущность.
   * @memberof EntityRepositoryService
   */
  createApiEntity(apiServiceName: string, entity: Entity): Observable<Entity> {
    return this.entityService.createApiEntity(apiServiceName, entity);
  }

  /**
   * Обновляет существующую сущность для указанного API сервиса.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @param {Entity} entity - Новая структура сущности.
   * @returns {Observable<Entity>} Наблюдаемый объект, содержащий обновленную сущность.
   * @memberof EntityRepositoryService
   */
  updateApiEntity(apiServiceName: string, entityName: string, entity: Entity): Observable<Entity> {
    return this.entityService.updateApiEntity(apiServiceName, entityName, entity);
  }

  /**
   * Удаляет сущность по её имени для указанного API сервиса.
   *
   * @param {string} apiServiceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @returns {Observable<void>} Наблюдаемый объект, указывающий на успешное удаление.
   * @memberof EntityRepositoryService
   */
  deleteApiEntity(apiServiceName: string, entityName: string): Observable<void> {
    return this.entityService.deleteApiEntity(apiServiceName, entityName);
  }

  /**
   * Обновляет статус активности сущности.
   *
   * @param {string} serviceName - Имя API сервиса.
   * @param {string} entityName - Имя сущности.
   * @param {boolean} isActive - Новый статус активности.
   * @returns {Observable<any>} Наблюдаемый объект, указывающий на успешное обновление статуса.
   * @memberof EntityRepositoryService
   */
  updateEntityStatus(serviceName: string, entityName: string, isActive: boolean): Observable<any> {
    return this.entityService.updateEntityStatus(serviceName, entityName, isActive);
  }
}
