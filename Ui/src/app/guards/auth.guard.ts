import {CanActivateFn, Router} from '@angular/router';
import {AuthenticationService} from "../services/authentication.service";
import {inject} from "@angular/core";
import {LoggedInUser} from "../models/user.model";

export const authGuard: CanActivateFn = () => {
  const authService: AuthenticationService = inject(AuthenticationService);
  const router: Router = inject(Router);

  const loggedInUser: LoggedInUser | null = authService.user;

  if (loggedInUser) {
    return true;
  } else {
    router.navigate(['login']).then();
    return false;
  }
};
