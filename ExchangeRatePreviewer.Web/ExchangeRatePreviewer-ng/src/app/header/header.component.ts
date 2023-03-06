import { Component } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  message = "Returns a list containing exchange rates, as expressed in Litas per 1 currency unit, for the specified date.";
}
