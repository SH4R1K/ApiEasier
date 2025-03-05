import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { TuiButton } from '@taiga-ui/core';

@Component({
  selector: 'app-error-display',
  imports: [TuiButton],
  templateUrl: './error-display.component.html',
  styleUrls: ['./error-display.component.css'],
})
export class ErrorDisplayComponent implements OnInit, OnDestroy {
  errorCode!: string;
  errorMessage!: string;
  timeLeft: number = 20;
  private timer: any;
  private interval: any;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.errorCode = params['code'] || 'Unknown Error';
      this.errorMessage = params['message'] || 'Произошла непредвиденная ошибка.';
    });

    // Установите таймер на 20 секунд
    this.timer = setTimeout(() => {
      this.location.back();
    }, 20000);

    // Обновляйте таймер каждую секунду
    this.interval = setInterval(() => {
      if (this.timeLeft > 0) {
        this.timeLeft--;
      }
    }, 1000);
  }

  ngOnDestroy(): void {
    // Очистите таймер и интервал при уничтожении компонента
    if (this.timer) {
      clearTimeout(this.timer);
    }
    if (this.interval) {
      clearInterval(this.interval);
    }
  }

  goBack(): void {
    this.location.back();
  }
}
