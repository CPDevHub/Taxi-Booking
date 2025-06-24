import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PassengerSignupComponent } from './passenger-signup.component';

const routes: Routes = [
  {
    path: '',
    component: PassengerSignupComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PassengerSignupRoutingModule {}
