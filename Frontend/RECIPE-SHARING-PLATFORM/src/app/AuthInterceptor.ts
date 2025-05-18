import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  // For GET requests, specify the endpoints to skip token attachment.
  private skipGetEndpoints = ['/recipes', '/likes', '/comments'];

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Only check skip for GET requests.
    if (req.method === 'GET') {
      const shouldSkip = this.skipGetEndpoints.some(urlPart => req.url.includes(urlPart));
      if (shouldSkip) {
        return next.handle(req);
      }
    }

    // For non-GET or GETs that don’t match skip list, add token if available.
    const token = localStorage.getItem('token');
    if (token) {
      const cloned = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
      return next.handle(cloned);
    }
    return next.handle(req);
  }
}