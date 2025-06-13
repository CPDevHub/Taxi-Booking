import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DriverSignupComponent } from './features/auth/driver-signup/driver-signup.component';
import { LoginComponent } from './features/auth/login/login.component';
import { PassengerSignupComponent } from './features/auth/passenger-signup/passenger-signup.component';
import { PageNotFoundComponent } from './core/pages/page-not-found/page-not-found.component';
import { UnauthorizedComponent } from './core/pages/unauthorized/unauthorized.component';
import { DriverHomeComponent } from './features/driver/home/home.component';
import { DashboardComponent } from './features/driver/dashboard/dashboard.component';
import { DriverSettingsComponent } from './features/driver/settings/settings.component';
import { PassengerHomeComponent } from './features/passenger/home/home.component';

import { PassengerSettingsComponent } from './features/passenger/settings/settings.component';
import { AuthGuardService } from './features/auth/auth-guard.service';
import { RideHistoryComponent } from './core/pages/history/history.component';
import { PassengerLayoutComponent } from './features/passenger/passenger-layout/passenger-layout.component';
import { DriverLayoutComponent } from './features/driver/driver-layout/driver-layout.component';

const routes: Routes = [
  { path: '', redirectTo: 'passenger-login', pathMatch: 'full' },
  { path: 'passenger-login', component: LoginComponent },
  { path: 'passenger-signup', component: PassengerSignupComponent },
  { path: 'driver-login', component: LoginComponent },
  { path: 'driver-signup', component: DriverSignupComponent },
  { path: 'unauthorized', component: UnauthorizedComponent },
  {
    path: 'driver',
    component: DriverLayoutComponent,
    canActivate: [AuthGuardService],
    data: { expectedRole: 'Driver' },
    children: [
      {
        path: '',
        component: DriverHomeComponent,
      },
      {
        path: 'dashboard',
        component: DashboardComponent,
      },
      {
        path: 'history',
        component: RideHistoryComponent,
      },
      {
        path: 'settings',
        component: DriverSettingsComponent,
      },
    ],
  },
  {
    path: 'passenger',
    component: PassengerLayoutComponent,
    canActivate: [AuthGuardService],
    data: { expectedRole: 'Passenger' },
    children: [
      {
        path: '',
        component: PassengerHomeComponent,
      },
      {
        path: 'history',
        component: RideHistoryComponent,
      },
      {
        path: 'settings',
        component: PassengerSettingsComponent,
      },
    ],
  },

  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
