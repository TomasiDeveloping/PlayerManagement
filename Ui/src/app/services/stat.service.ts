import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {map, Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class StatService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'Stats/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getAllianceUseCount() : Observable<number> {
    return this._httpClient.get<{amount: number}>(this._serviceUrl + 'useCount')
      .pipe(
        map(res => res.amount)
      )
  }

}
