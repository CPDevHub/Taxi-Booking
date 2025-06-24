import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { finalize, Observable } from 'rxjs';
import { LoaderService } from '../services/loader.service';
import { Injectable } from '@angular/core';
import { ACCESS_TOKEN } from 'src/app/shared/constants/token';


@Injectable() 
export class CredentialInterceptor implements HttpInterceptor {
  constructor(private loaderService: LoaderService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const token = sessionStorage.getItem(ACCESS_TOKEN) || '';

    let clonedRequest = req;

    if (token) {
      clonedRequest = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
        withCredentials: true,
      });
    } else {
      clonedRequest = req.clone({
        withCredentials: true,
      });
    }
    this.loaderService.show();

    return next.handle(clonedRequest).pipe(
      finalize(() => {
        this.loaderService.hide();
      })
    );
  }
}
