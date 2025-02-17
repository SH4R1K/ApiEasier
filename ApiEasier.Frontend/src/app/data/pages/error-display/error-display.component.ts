import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TuiButton } from '@taiga-ui/core';

@Component({
  selector: 'app-error-display',
  imports: [TuiButton],
  templateUrl: './error-display.component.html',
  styleUrl: './error-display.component.css',
})
export class ErrorDisplayComponent implements OnInit {
  errorCode!: string;
  errorMessage!: string;

  constructor(private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.errorCode = params['code'] || 'Unknown Error';
      this.errorMessage =
        params['message'] || 'Произошла непредвиденная ошибка.';
    });
  }

  goBack(): void {
    this.router.navigate(['/']);
  }
}
