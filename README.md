# ExchangeRatePreviewer

#### Calls Bank of Lithuania API for the change in currency exchange rates for a specific date, supplied by the user. Dates are added from the datepicker tool in the user interface. The accepted date range is 2000-01-01 to 2014-12-31.

### Running the app:

- Restore and build the solution
- Run the ExchangeRatePreviewer.API project
    - Thest the API endpoint on Postman for the date below by sending a HTTP GET request to https://localhost:7026/lb/exchangeRates?date=2014-12-04
    - For swagger, navigate to https://localhost:7026/swagger
- To run the front-end, in the command prompt navigate to *./ExchangeRatePreviewer.Web/ExchangeRatePreviewer-ng/* and run `ng serve`. The app can be viewed at http://localhost:4200.
    - Choose a date from the datepicker control and wait for the results to be displayed.

### Project architecture

A strategy pattern was implemented for services in the business logic (the Core project). The context comes in from the controller in the application layer (the API project). Dependency Injection technique was used to achieve inversion of control between classes and their dependencies. A singleton design pattern applied for loading the HTTP client settings from the appsettings.json document. 
Iterator pattern applied when processing Bank of Lithuania web service's response data, parsing it into a list of `ExchangeRateDto` objects.

For the front-end, the *Angular* *JavaScript* framework was used. Its data binding and component functionality allowed for responsive, reusable view element generation, based on the data received from the request made to the API. In addition the *Angular Material* library was utilised to simplify date selection and the display of response items (exchange rates). 