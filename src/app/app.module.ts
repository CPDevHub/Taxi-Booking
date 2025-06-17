import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ButtonsModule } from '@progress/kendo-angular-buttons';
import { InputsModule } from '@progress/kendo-angular-inputs';
import { DriverSignupComponent } from './features/auth/driver-signup/driver-signup.component';
import { PassengerSignupComponent } from './features/auth/passenger-signup/passenger-signup.component';
import { LoginComponent } from './features/auth/login/login.component';
import { FloatingLabelModule } from '@progress/kendo-angular-label';
import {
  StackLayoutModule,
  CardModule,
  GridLayoutModule,
} from '@progress/kendo-angular-layout';
import {
  ComboBoxModule,
  DropDownListModule,
} from '@progress/kendo-angular-dropdowns';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { CredentialInterceptor } from './core/interceptors/credentialsInterceptor.interceptor';
import { UnauthorizedComponent } from './core/pages/unauthorized/unauthorized.component';
import { PageNotFoundComponent } from './core/pages/page-not-found/page-not-found.component';
import { DashboardComponent } from './features/driver/dashboard/dashboard.component';
import { DriverHomeComponent } from './features/driver/home/home.component';
import { DriverSettingsComponent } from './features/driver/settings/settings.component';

import { PassengerHomeComponent } from './features/passenger/home/home.component';
import { PassengerSettingsComponent } from './features/passenger/settings/settings.component';
import { IconsModule } from '@progress/kendo-angular-icons';
import { AppBarModule } from '@progress/kendo-angular-navigation';
import { ToastrModule } from 'ngx-toastr';
import { DialogModule } from '@progress/kendo-angular-dialog';
import { ModalComponent } from './core/components/modal/modal.component';
import { MapComponent } from './core/components/map/map.component';
import { SidebarComponent } from './features/passenger/home/sidebar/sidebar.component';
import { RideHistoryComponent } from './core/pages/history/history.component';
import { GridModule } from '@progress/kendo-angular-grid';
import { PassengerLayoutComponent } from './features/passenger/passenger-layout/passenger-layout.component';
import { DriverLayoutComponent } from './features/driver/driver-layout/driver-layout.component';
import { RatingComponent } from './core/components/rating/rating.component';


@NgModule({
  declarations: [
    AppComponent,
    DriverSignupComponent,
    PassengerSignupComponent,
    LoginComponent,
    UnauthorizedComponent,
    PageNotFoundComponent,
    DashboardComponent,
    DriverHomeComponent,
    PassengerHomeComponent,
    DriverSettingsComponent,
    PassengerSettingsComponent,
    ModalComponent,
    MapComponent,
    SidebarComponent,
    RideHistoryComponent,
    PassengerLayoutComponent,
    DriverLayoutComponent,
    RatingComponent,
    
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    StackLayoutModule,
    ButtonsModule,
    InputsModule,
    CardModule,
    FloatingLabelModule,
    DropDownListModule,
    BrowserAnimationsModule,
    IconsModule,
    ComboBoxModule,
    DialogModule,
    AppBarModule,
    GridModule,
    ToastrModule.forRoot({
      positionClass: 'toast-top-center',
      timeOut: 3000,
      closeButton: true,
      progressBar: true,
    }),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      multi: true,
      useClass: CredentialInterceptor,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
