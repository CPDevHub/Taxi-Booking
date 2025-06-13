import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { VehicleType } from 'src/app/shared/enums/vehicleType.enums';
import { AuthService } from '../auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-driver-signup',
  templateUrl: './driver-signup.component.html',
  styleUrls: ['./driver-signup.component.css'],
})
export class DriverSignupComponent implements OnInit {
  carTypes: string[] = Object.keys(VehicleType);

  reactiveForm!: FormGroup;

  constructor(
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.reactiveForm = new FormGroup({
      email: new FormControl(null, [Validators.required]),
      password: new FormControl(null, [Validators.required]),
      name: new FormControl(null, [Validators.required]),
      avatarUrl: new FormControl(null, [Validators.required]),
      contactNumber: new FormControl(null, [Validators.required]),
      vehicleNumber: new FormControl(null, [Validators.required]),
      driverVehicleType: new FormControl(VehicleType.Hatchback),
      vehicleModel: new FormControl(null, [Validators.required]),
    });
  }

  onDriverSignup() {
    if (this.reactiveForm.valid) {
      this.authService.driverSignup(this.reactiveForm.value).subscribe({
        next: (res) => {
          this.router.navigateByUrl('driver-login');
        },
        error: (err) => {
          this.toastr.error(err.error.message);
        },
      });
    }
  }
}
