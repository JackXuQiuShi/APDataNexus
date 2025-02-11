import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, finalize } from 'rxjs';
import { ProgressService } from './services/progress.service';


@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private progressService: ProgressService) { }

  Token!: string;

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.progressService.show(); // Show progress indicator

    const userString = sessionStorage.getItem('user');
    let authReq = request;

    if (userString) {
      const user = JSON.parse(userString);
      const token = user.Token;

      if (token) {
        authReq = request.clone({
          headers: request.headers
            .set('Authorization', `Bearer ${token}`)
            .set('Content-Type', 'application/json')
        });
      }
    }

    return next.handle(authReq).pipe(
      finalize(() => this.progressService.hide()) // Hide progress indicator regardless of request outcome
    );
  }




}
