import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SignalrService } from 'src/app/core/services/signalrService.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  reactiveForm!: FormGroup;
  userType!: 'passenger' | 'driver';

  constructor(
    private authService: AuthService,
    private router: Router,
    private signarRService: SignalrService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.reactiveForm = new FormGroup({
      email: new FormControl(null, [Validators.required]),
      password: new FormControl(null, [Validators.required]),
    });

    const url = this.router.url;
    this.userType = url.includes('passenger') ? 'passenger' : 'driver';
  }

  onLogin() {
    if (this.reactiveForm.valid) {
      this.authService.login(this.reactiveForm.value, this.userType).subscribe({
        next: (res: any) => {
          if (this.userType === 'passenger')
            this.signarRService.loginPassenger(res.data.response.id);
          else this.signarRService.loginDriver(res.data.response.id);

          sessionStorage.setItem('access_token', res.data.token);
          this.signarRService.connect();
          this.router.navigateByUrl(`${this.userType}`);
        },
        error: (err) => {
          this.toastr.error(err.error.message);
        },
      });
    }
  }
}
