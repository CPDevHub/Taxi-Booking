import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PassengerSignupRoutingModule } from './passenger-signup-routing.module';
import { PassengerSignupComponent } from './passenger-signup.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [PassengerSignupComponent],
  imports: [CommonModule, PassengerSignupRoutingModule,SharedModule,ReactiveFormsModule],
})
export class PassengerSignupModule {}
