import { Component, OnInit } from '@angular/core';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent  implements OnInit {

  minDate: Date;
  maxDate: Date;

  constructor () {
    this.minDate = new Date("2000-01-01")
    this.maxDate = new Date("2014-12-31");
  };

  ngOnInit() : void {
  }

  message: string = 'Returns a list containing exchange rates, as expressed in Litas per 1 currency unit, for the specified date.';
  imageSource: string = '/assets/currencies.jpg';
  dateValue: string = '';

  passDate(eventData: MatDatepickerInputEvent<Date>): string {
      const year: string = '' + eventData?.value?.getFullYear();

      let month: string = '';
      if (eventData.value?.getMonth()) {
        month = '' + (eventData.value?.getMonth() + 1);
        if (month.length < 2) {
          month = '0' + month;
        }
      }

      let day: string = '' + eventData.value?.getDate();
      if (day.length < 2) {
        day = '0' + day;
      }

      console.log(year + '-' + month + '-' + day);

      return year + '-' + month + '-' + day;
  };
}
