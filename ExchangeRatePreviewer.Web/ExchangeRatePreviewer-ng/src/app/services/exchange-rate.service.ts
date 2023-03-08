import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { HttpHeaders } from "@angular/common/http";
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { HandleError, HttpErrorHandler } from "./http-error-handler.service";
import { ExchangeRate } from "../exchange-rates/exchange-rate";

@Injectable()
export class ExchangeRateService{
  exchangeRateServiceUrl: string = 'https://localhost:7026/lb/exchangeRates';
  private handleError: HandleError;

  constructor(
    private http: HttpClient,
    httpErrorHandler: HttpErrorHandler) {
      this.handleError = httpErrorHandler.createHandleError('ExchangeRateService');
  }

  getExchangeRatesByDate(date: string): Observable<ExchangeRate[]> {
    date = date.trim();

    const options = date ? {params: new HttpParams().set('date', date)} : {};

    return this.http.get<ExchangeRate[]>(this.exchangeRateServiceUrl, options)
      .pipe(
        catchError(this.handleError<ExchangeRate[]>('getExchangeRatesByDate', []))
      );
  }
}
