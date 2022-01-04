import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root',
})
export class SpinnerService {
  constructor(private spinner: NgxSpinnerService) {}

  private counter = 0;

  display() {
    this.counter++;
    this.spinner.show(undefined, { type: 'ball-atom', fullScreen: true, color: '#2f53a7' });
  }

  conceal() {
    this.counter--;
    if (this.counter <= 0) {
      this.counter = 0;
      this.spinner.hide();
    }
  }
}
