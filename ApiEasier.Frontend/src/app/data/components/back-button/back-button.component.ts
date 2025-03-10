import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-back-button',
  imports: [],
  templateUrl: './back-button.component.html',
  styleUrl: './back-button.component.css'
})
export class BackButtonComponent {
  constructor(private location: Location, private router: Router) {}

  goBack(): void {
    try {
      const previousUrl = this.location.path(true); 
      const baseDomain = '/'; 
      if (previousUrl.startsWith(baseDomain)) {
        this.location.back(); 
      } else {
        this.router.navigateByUrl('/');
      }
    } catch (error) {
      this.router.navigateByUrl('/');
    }
  }

}