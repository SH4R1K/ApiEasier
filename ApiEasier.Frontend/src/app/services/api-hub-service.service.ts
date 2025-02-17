import { Injectable, Input, NgZone } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { apiServiceShortStructure } from './service-structure-api';

@Injectable({
  providedIn: 'root'
})
export class ApiHubServiceService {
  private hubConnection!: signalR.HubConnection;
  private apiListSubject = new BehaviorSubject<apiServiceShortStructure[]>([]);
  private baseUrl = `${window.location.origin}/hubs`;
  ordersUpdated$: Observable<apiServiceShortStructure[]> = this.apiListSubject.asObservable();
  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`hubs/apilisthub`, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect([1000, 3000, 5000])
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connected to SignalR hub'))
      .catch(err => console.error('Error connecting to SignalR hub:', err));

    this.hubConnection.on('RecieveMessage', (apiList: apiServiceShortStructure[]) => {
      this.apiListSubject.next(apiList);
      console.log(apiList);
    });

    this.hubConnection.on('AddService', (api: apiServiceShortStructure) => {
      const currentList = this.apiListSubject.getValue();
      currentList.push(api);
      this.apiListSubject.next(currentList);
      console.log(currentList);
    });
    
    this.hubConnection.on('UpdateService', (oldName: string, api: apiServiceShortStructure) => {
      const currentList = this.apiListSubject.getValue();
      const index = currentList.findIndex((apiService: apiServiceShortStructure) => {
        return apiService.name === oldName
      });
      currentList[index] = api;
      this.apiListSubject.next(currentList);
    });
    
    this.hubConnection.on('RemoveService', (name: string) => {
      const currentList = this.apiListSubject.getValue();
      const index = currentList.findIndex((apiService: apiServiceShortStructure) => {
        return apiService.name === name
      });
      currentList.splice(index, 1);
      this.apiListSubject.next(currentList);
    });

    this.hubConnection.on('UpdateStatusService', (name: string, isActive: boolean) => {
      this.apiListSubject.next(
        this.apiListSubject.getValue().map(apiService => 
          apiService.name === name ? { ...apiService, isActive } : apiService
        )
      );
    });
  }
  
  initializeData(initialData: apiServiceShortStructure[]) {
    this.apiListSubject.next(initialData);
  }
}
