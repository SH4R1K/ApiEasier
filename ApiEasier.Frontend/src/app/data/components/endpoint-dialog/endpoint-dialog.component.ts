import type { TemplateRef } from '@angular/core';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TuiAutoFocus } from '@taiga-ui/cdk';
import type { TuiDialogContext } from '@taiga-ui/core';
import { TuiButton, TuiDialogService, TuiTextfield } from '@taiga-ui/core';
import { TuiDataListWrapper, TuiSlider } from '@taiga-ui/kit';
import {
  TuiInputModule,
  TuiSelectModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/legacy';
import { injectContext } from '@taiga-ui/polymorpheus';
import { Endpoint } from "../../../interfaces/Endpoint";

@Component({
  selector: 'app-endpoint-dialog',
  imports: [
    FormsModule,
    TuiAutoFocus,
    TuiButton,
    TuiDataListWrapper,
    TuiInputModule,
    TuiSelectModule,
    TuiSlider,
    TuiTextfield,
    TuiTextfieldControllerModule,
  ],
  templateUrl: './endpoint-dialog.component.html',
  styleUrls: ['./endpoint-dialog.component.css'],
})
export class EndpointDialogComponent {
  private readonly dialogs = inject(TuiDialogService);
  readonly types: string[] = ['get', 'post', 'put', 'delete', 'getbyindex'];

  public readonly context =
    injectContext<TuiDialogContext<Endpoint, Endpoint>>();

  protected get hasValue(): boolean {
    return this.data.route.trim() !== '';
  }

  protected get data(): Endpoint {
    return this.context.data;
  }

  protected submit(): void {
    if (event) {
      event.preventDefault();
    }
    if (this.hasValue) {
      this.context.completeWith(this.data);
    }
  }

  protected showDialog(content: TemplateRef<TuiDialogContext>): void {
    this.dialogs.open(content, { dismissible: true }).subscribe();
  }

  protected onInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    const value = input.value;
    const cleanedValue = value.replace(/[^a-zA-Z0-9]/g, '');
    input.value = cleanedValue;
    this.data.route = cleanedValue;
  }
}
