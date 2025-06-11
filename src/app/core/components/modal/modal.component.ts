import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { RideCancellationReason } from 'src/app/shared/enums/rideCancellationReason.enum';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css'],
})
export class ModalComponent {
  @Input() rideId!: number;
  @Output() close = new EventEmitter<void>();
  @Output() submit = new EventEmitter<{ rideId: number; reason: string }>();

  selectedReason!: string;
  cancellationReasons: string[] = Object.values(RideCancellationReason);
  selectReason(reason: string) {
    this.selectedReason = reason;
  }

  onSubmit(): void {
    if (this.selectedReason) {
      this.submit.emit({ rideId: this.rideId, reason: this.selectedReason });
      this.close.emit();
    }
  }

  onClose(): void {
    this.close.emit();
  }
}
