import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { AlertDeleteComponent } from "../../components/alert-delete/alert-delete.component";
import { HeaderComponent } from '../../components/header/header.component';
import { TuiButton } from '@taiga-ui/core';

@Component({
  selector: 'app-page-not-found',
  imports: [
    CommonModule,
     TuiButton,
     RouterModule
],
  templateUrl: './page-not-found.component.html',
  styleUrl: './page-not-found.component.css'
})
export class PageNotFoundComponent {

constructor(
   private cd: ChangeDetectorRef,
    private route: ActivatedRoute,
  ) {}
}
