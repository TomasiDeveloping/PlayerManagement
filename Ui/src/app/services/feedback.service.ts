import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class FeedbackService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'Feedbacks/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  public submitFeedback(formData: FormData): Observable<{url:string}> {
    return this._httpClient.post<{url:string}>(this._serviceUrl, formData);
  }

}
