import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { CredentialInterceptor } from './core/interceptors/credentialsInterceptor.interceptor';
import { UnauthorizedComponent } from './core/pages/unauthorized/unauthorized.component';
import { PageNotFoundComponent } from './core/pages/page-not-found/page-not-found.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ToastrModule } from 'ngx-toastr';

import { SharedModule } from './shared/shared.module';
import { LoaderComponent } from './core/components/loader/loader.component';

@NgModule({
  declarations: [
    AppComponent,
    UnauthorizedComponent,
    PageNotFoundComponent,
    LoaderComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    SharedModule,
    FormsModule,
    BrowserAnimationsModule,
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
