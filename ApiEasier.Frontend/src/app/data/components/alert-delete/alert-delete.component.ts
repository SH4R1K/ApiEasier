import { CommonModule, NgForOf } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import type { TuiPopover } from '@taiga-ui/cdk';
import type { TuiAlertOptions } from '@taiga-ui/core';
import { TuiAlertService } from '@taiga-ui/core';
import { injectContext, PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { switchMap, takeUntil } from 'rxjs';

@Component({
  selector: 'app-alert-on-delete',
  imports: [CommonModule],
  templateUrl: './alert-delete.component.html',
  styleUrl: './alert-delete.component.css'
})
export class AlertDeleteComponent {
  protected readonly context =
  injectContext<TuiPopover<TuiAlertOptions<void>, boolean>>();
}