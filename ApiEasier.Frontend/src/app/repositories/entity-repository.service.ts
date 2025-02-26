import { Injectable } from '@angular/core';
import { EntityService } from '../services/entity-service.service';
import { Observable } from 'rxjs';
import { Entity } from "../interfaces/Entity";

@Injectable({
  providedIn: 'root'
})
export class EntityRepositoryService {

  constructor(private entityService: EntityService) { }

  getApiEntityList(apiServiceName: string): Observable<Entity[]> {
    return this.entityService.getApiEntityList(apiServiceName);
  } 

  getApiEntity(apiServiceName: string, entityName: string): Observable<Entity> {
    return this.entityService.getApiEntity(apiServiceName, entityName);
  }

  createApiEntity(apiServiceName: string, entity: Entity): Observable<Entity> {
    return this.entityService.createApiEntity(apiServiceName, entity);
  }

  updateApiEntity(apiServiceName: string, entityName: string, entity: Entity): Observable<Entity> {
    return this.entityService.updateApiEntity(apiServiceName, entityName, entity);
  }

  deleteApiEntity(apiServiceName: string, entityName: string): Observable<void> {
    return this.entityService.deleteApiEntity(apiServiceName, entityName);
  } 

  updateEntityStatus(serviceName: string, entityName: string, isActive: boolean): Observable<any> {
    return this.entityService.updateEntityStatus(serviceName, entityName, isActive);
  }

}
