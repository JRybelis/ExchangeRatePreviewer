import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Exchange Rate Previewer';
  message = "Returns a list containing exchange rates, as expressed in Litas per 1 currency unit, for the specified date.";
}
