import { HttpInterceptorFn } from '@angular/common/http';
import {SpinnerService} from "../services/spinner.service";
import {inject} from "@angular/core";
import {finalize} from "rxjs";

export const spinnerInterceptor: HttpInterceptorFn = (req, next) => {
  const spinnerService: SpinnerService = inject(SpinnerService);

  if(req.url.includes('Stats/useCount')) {
    return next(req);
  }

  spinnerService.busy();

  return next(req).pipe(
    finalize(() => {
      spinnerService.idle()
    })
  );
};
