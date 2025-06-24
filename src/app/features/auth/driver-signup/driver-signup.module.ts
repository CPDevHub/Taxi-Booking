import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DriverSignupRoutingModule } from './driver-signup-routing.module';
import { DriverSignupComponent } from './driver-signup.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    DriverSignupComponent
  ],
  imports: [
    CommonModule,
    DriverSignupRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ]
})
export class DriverSignupModule { }
