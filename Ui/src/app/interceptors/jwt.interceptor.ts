import { HttpInterceptorFn } from '@angular/common/http';
import {JwtTokenService} from "../services/jwt-token.service";
import {inject} from "@angular/core";

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const jwtService: JwtTokenService = inject(JwtTokenService);

  const token: string | null = jwtService.getToken();

  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
    }
    });
  }

  return next(req);
};
