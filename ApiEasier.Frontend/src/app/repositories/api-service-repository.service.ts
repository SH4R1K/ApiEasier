import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  apiServiceShortStructure,
  ApiServiceStructure,
} from '../services/service-structure-api';
import { ApiService } from '../services/api-service.service';

@Injectable({
  providedIn: 'root',
})
export class ApiServiceRepositoryService {
  constructor(private apiService: ApiService) {}

  getApiList(): Observable<apiServiceShortStructure[]> {
    return this.apiService.getApiList();
  }

  getApiStructureList(name: string): Observable<ApiServiceStructure> {
    return this.apiService.getApiStructureList(name);
  }

  createApiService(
    service: apiServiceShortStructure
  ): Observable<apiServiceShortStructure> {
    return this.apiService.createApiService(service);
  }

  createFullApiService(service: ApiServiceStructure): Observable<void> {
    return this.apiService.createFullApiService(service);
  }

  updateApiService(
    oldName: string,
    service: apiServiceShortStructure
  ): Observable<apiServiceShortStructure> {
    return this.apiService.updateApiService(oldName, service);
  }

  deleteApiService(serviceName: string): Observable<void> {
    return this.apiService.deleteApiService(serviceName);
  }

  updateApiServiceStatus(
    serviceName: string,
    isActive: boolean
  ): Observable<any> {
    return this.apiService.updateApiServiceStatus(serviceName, isActive);
  }
}
