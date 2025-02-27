import { Injectable, Input, NgZone } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { apiServiceShortStructure } from "../interfaces/apiServiceShortStructure";

/**
 * Сервис для взаимодействия с SignalR хабом для управления API сервисами.
 *
 * @remarks
 * Этот сервис управляет подключением к SignalR хабу и обрабатывает события, связанные с API сервисами.
 * Он использует BehaviorSubject для управления состоянием списка API сервисов.
 *
 * @type {ApiHubServiceService}
 * @memberof Component
 */
@Injectable({
  providedIn: 'root'
})
export class ApiHubServiceService {
  private hubConnection!: signalR.HubConnection;
  private apiListSubject = new BehaviorSubject<apiServiceShortStructure[]>([]);
  private baseUrl = `${window.location.origin}/hubs`;

  /**
   * Наблюдаемый объект, содержащий обновленный список API сервисов.
   *
   * @type {Observable<apiServiceShortStructure[]>}
   * @memberof ApiHubServiceService
   */
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

    /**
     * Обработчик события получения списка API сервисов.
     *
     * @param {apiServiceShortStructure[]} apiList - Список API сервисов.
     * @listens RecieveMessage
     * @memberof ApiHubServiceService
     */
    this.hubConnection.on('RecieveMessage', (apiList: apiServiceShortStructure[]) => {
      this.apiListSubject.next(apiList);
      console.log(apiList);
    });

    /**
     * Обработчик события добавления нового API сервиса.
     *
     * @param {apiServiceShortStructure} api - Новый API сервис.
     * @listens AddService
     * @memberof ApiHubServiceService
     */
    this.hubConnection.on('AddService', (api: apiServiceShortStructure) => {
      const currentList = this.apiListSubject.getValue();
      currentList.push(api);
      this.apiListSubject.next(currentList);
      console.log(currentList);
    });

    /**
     * Обработчик события обновления существующего API сервиса.
     *
     * @param {string} oldName - Старое имя API сервиса.
     * @param {apiServiceShortStructure} api - Обновленный API сервис.
     * @listens UpdateService
     * @memberof ApiHubServiceService
     */
    this.hubConnection.on('UpdateService', (oldName: string, api: apiServiceShortStructure) => {
      const currentList = this.apiListSubject.getValue();
      const index = currentList.findIndex((apiService: apiServiceShortStructure) => {
        return apiService.name === oldName
      });
      currentList[index] = api;
      this.apiListSubject.next(currentList);
    });

    /**
     * Обработчик события удаления API сервиса.
     *
     * @param {string} name - Имя удаляемого API сервиса.
     * @listens RemoveService
     * @memberof ApiHubServiceService
     */
    this.hubConnection.on('RemoveService', (name: string) => {
      const currentList = this.apiListSubject.getValue();
      const index = currentList.findIndex((apiService: apiServiceShortStructure) => {
        return apiService.name === name
      });
      currentList.splice(index, 1);
      this.apiListSubject.next(currentList);
    });

    /**
     * Обработчик события обновления статуса активности API сервиса.
     *
     * @param {string} name - Имя API сервиса.
     * @param {boolean} isActive - Новый статус активности.
     * @listens UpdateStatusService
     * @memberof ApiHubServiceService
     */
    this.hubConnection.on('UpdateStatusService', (name: string, isActive: boolean) => {
      this.apiListSubject.next(
        this.apiListSubject.getValue().map(apiService =>
          apiService.name === name ? { ...apiService, isActive } : apiService
        )
      );
    });
  }

  /**
   * Инициализирует данные списка API сервисов.
   *
   * @param {apiServiceShortStructure[]} initialData - Начальные данные списка API сервисов.
   * @memberof ApiHubServiceService
   */
  initializeData(initialData: apiServiceShortStructure[]) {
    this.apiListSubject.next(initialData);
  }
}
