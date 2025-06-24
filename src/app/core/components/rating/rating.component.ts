import { Component, EventEmitter, Input, Output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DriverService } from '../../services/driver.service';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-rating',
  templateUrl: './rating.component.html',
  styleUrls: ['./rating.component.css'],
})
export class RatingComponent {
  @Input() driverId: number | undefined;
  @Output() submitted = new EventEmitter();
  rating = 0;

  constructor(private driverService: DriverService) {}

  setRating(r: number) {
    this.rating = r;
  }

  submitRating() {
    if (!this.rating) return;
    if (!this.driverId) return;
    this.driverService
      .submitRating(this.driverId, this.rating)
      .subscribe(() => {
        this.submitted.emit();
      });
  }
}
