import { Injectable } from '@angular/core';
import { EndpointService as EndpointService } from '../services/endpoint-service.service';
import { Endpoint as Endpoint } from "../interfaces/Endpoint";
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EndpointRepositoryService {

  constructor(private endpointService: EndpointService) { }

  getEndpointList(apiServiceName: string, entityName: string): Observable<Endpoint[]> {
    return this.endpointService.getEndpointList(apiServiceName, entityName);
  }

  createEndpoint(apiServiceName: string, entityName: string, endpoint: Endpoint): Observable<Endpoint> {
    return this.endpointService.createEndpoint(apiServiceName, entityName, endpoint);
  }

  getEndpointByName(apiServiceName: string, entityName: string, endpointName: string): Observable<Endpoint> {
    return this.endpointService.getEndpointByName(apiServiceName, entityName, endpointName);
  }

  updateEndpoint(apiServiceName: string, entityName: string, endpointName: string, endpoint: Endpoint): Observable<Endpoint> {
    return this.endpointService.updateEndpoint(apiServiceName, entityName, endpointName, endpoint);
  }

  deleteEndpoint(apiServiceName: string, entityName: string, endpointName: string): Observable<void> {
    return this.endpointService.deleteEndpoint(apiServiceName, entityName, endpointName);
  }
  
  updateEndpointStatus(apiServiceName: string, entityName: string, endpointName: string, isActive: boolean): Observable<any> {
    return this.endpointService.updateEndpointStatus(apiServiceName, entityName, endpointName, isActive);
  }

}
