import { Component, ElementRef, ViewChild, OnInit, Input } from '@angular/core';
import { ExchangeRate } from './exchange-rate';
import { ExchangeRateService } from '../services/exchange-rate.service';

@Component({
  selector: 'app-exchange-rates',
  templateUrl: './exchange-rates.component.html',
  providers: [ExchangeRateService],
  styleUrls: ['./exchange-rates.component.css']
})
export class ExchangeRatesComponent implements OnInit{
  exchangeRates: ExchangeRate[] = [];

  constructor(private exchangeRateService: ExchangeRateService){ }

  ngOnInit(): void {
    // this.getExchangeRatesByDate('2014-12-04');
  }


  getExchangeRatesByDate(exchangeRateDate: string): void {
    this.exchangeRateService.getExchangeRatesByDate(exchangeRateDate)
      .subscribe((exchangeRates: ExchangeRate[]) => {
        this.exchangeRates = exchangeRates;
        console.info(this.exchangeRates);
      });
  }
}
