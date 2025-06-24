import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from '../auth/auth-guard.service';
import { DriverHomeComponent } from './home/home.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { DriverSettingsComponent } from './settings/settings.component';
import { RideHistoryComponent } from 'src/app/core/pages/history/history.component';
import { DriverLayoutComponent } from './driver-layout/driver-layout.component';

const routes: Routes = [
  {
    path: '',
    component: DriverLayoutComponent,
    canActivate: [AuthGuardService],
    children: [
      { path: '', component: DriverHomeComponent },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'settings', component: DriverSettingsComponent },
      { path: 'history', component: RideHistoryComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DriverRoutingModule {}
