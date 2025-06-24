import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DriverSignupComponent } from './driver-signup.component';

const routes: Routes = [
  {
    path:'',component:DriverSignupComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DriverSignupRoutingModule { }
