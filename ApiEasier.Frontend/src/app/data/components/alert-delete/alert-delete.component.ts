import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import type { TuiPopover } from '@taiga-ui/cdk';
import type { TuiAlertOptions } from '@taiga-ui/core';
import { injectContext } from '@taiga-ui/polymorpheus';

/**
 * Компонент AlertDeleteComponent предназначен для отображения уведомления при выполнении действия удаления.
 * Использует компоненты Taiga UI для управления всплывающим окном уведомления и его опциями.
 *
 * @remarks
 * Этот компонент является частью системы уведомлений и используется для оповещения пользователей о действиях удаления.
 * Интегрируется с сервисом уведомлений Taiga UI для обеспечения единообразного пользовательского опыта.
 *
 * @example
 * html
 * <app-alert-on-delete></app-alert-on-delete>
 *
 */
@Component({
  selector: 'app-alert-on-delete',
  imports: [CommonModule],
  templateUrl: './alert-delete.component.html',
  styleUrls: ['./alert-delete.component.css'], 
})
export class AlertDeleteComponent {
  /**
   * Контекст для компонента TuiPopover, который управляет опциями уведомления.
   *
   * @remarks
   * Этот контекст внедряется с использованием функции injectContext из Taiga UI.
   * Он предоставляет необходимую конфигурацию для всплывающего окна уведомления.
   *
   * @type {TuiPopover<TuiAlertOptions<void>, boolean>}
   * @default undefined
   * @memberof AlertDeleteComponent
   */
  protected readonly context = injectContext<TuiPopover<TuiAlertOptions<void>, boolean>>();
}
