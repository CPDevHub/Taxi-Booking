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
import { HistoryComponent } from './features/passenger/history/history.component';
import { PassengerSettingsComponent } from './features/passenger/settings/settings.component';
import { AuthGuardService } from './features/auth/auth-guard.service';

const routes: Routes = [
  { path: '', redirectTo: 'passenger-login', pathMatch: 'full' },
  { path: 'passenger-login', component: LoginComponent },
  { path: 'passenger-signup', component: PassengerSignupComponent },
  { path: 'driver-login', component: LoginComponent },
  { path: 'driver-signup', component: DriverSignupComponent },
  { path: 'unauthorized', component: UnauthorizedComponent },
  {
    path: 'driver',
    component: DriverHomeComponent,
    canActivate: [AuthGuardService],
    data: { expectedRole: 'Driver' },
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
      },
      {
        path: 'settings',
        component: DriverSettingsComponent,
      },
    ],
  },
  {
    path: 'passenger',
    component: PassengerHomeComponent,
    canActivate: [AuthGuardService],
    data: { expectedRole: 'Passenger' },
    children: [
      {
        path: 'history',
        component: HistoryComponent,
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
