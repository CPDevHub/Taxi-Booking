import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PassengerLayoutComponent } from './passenger-layout/passenger-layout.component';
import { AuthGuardService } from '../auth/auth-guard.service';
import { PassengerHomeComponent } from './home/home.component';
import { PassengerSettingsComponent } from './settings/settings.component';
import { RideHistoryComponent } from 'src/app/core/pages/history/history.component';

const routes: Routes = [
  {
    path: '',
    component: PassengerLayoutComponent,
    canActivate: [AuthGuardService],
    children: [
      { path: '', component: PassengerHomeComponent },
      { path: 'settings', component: PassengerSettingsComponent },
      { path: 'history', component: RideHistoryComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PassengerRoutingModule {}
