import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import {  Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-passenger-signup',
  templateUrl: './passenger-signup.component.html',
  styleUrls: ['./passenger-signup.component.css'],
})
export class PassengerSignupComponent implements OnInit {
  reactiveForm!: FormGroup;

  constructor(private authService: AuthService,private toastr: ToastrService,
      private router: Router) {}

  ngOnInit(): void {
    this.reactiveForm = new FormGroup({
      email: new FormControl(null, [Validators.required]),
      password: new FormControl(null, [Validators.required]),
      name: new FormControl(null, [Validators.required]),
      avatarUrl: new FormControl(null),
      contactNumber: new FormControl(null, [Validators.required]),
    });
  }

  onPassengerSignup() {
    if (this.reactiveForm.valid) {
      this.authService
        .passengerSignup(this.reactiveForm.value)
        .subscribe({
          next:(res) => {
          this.router.navigateByUrl('passenger-login')
        },
        error:(err)=>{
          this.toastr.error(err.error.message)
        }
        });
    }
  }
}
