import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PageNotFoundComponent } from './core/pages/page-not-found/page-not-found.component';
import { UnauthorizedComponent } from './core/pages/unauthorized/unauthorized.component';

const routes: Routes = [
  { path: '', redirectTo: 'passenger-login', pathMatch: 'full' },
  {
    path: 'passenger-login',
    loadChildren: () =>
      import('./features/auth/login/login.module').then(
        (mod) => mod.LoginModule
      ),
  },
  {
    path: 'passenger-signup',
    loadChildren: () =>
      import('./features/auth/passenger-signup/passenger-signup.module').then(
        (mod) => mod.PassengerSignupModule
      ),
  },
  {
    path: 'driver-login',
    loadChildren: () =>
      import('./features/auth/login/login.module').then(
        (mod) => mod.LoginModule
      ),
  },
  {
    path: 'driver-signup',
    loadChildren: () =>
      import('./features/auth/driver-signup/driver-signup.module').then(
        (mod) => mod.DriverSignupModule
      ),
  },
  { path: 'unauthorized', component: UnauthorizedComponent },
  {
    path: 'driver',
    data: { expectedRole: 'Driver' },
    loadChildren: () =>
      import('./features/driver/driver.module').then((mod) => mod.DriverModule),
  },
  {
    path: 'passenger',

    data: { expectedRole: 'Passenger' },
    loadChildren: () =>
      import('./features/passenger/passenger.module').then(
        (mod) => mod.PassengerModule
      ),
  },

  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
