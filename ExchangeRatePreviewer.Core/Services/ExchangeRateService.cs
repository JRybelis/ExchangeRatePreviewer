using ExchangeRatePreviewer.Core.Interfaces;
using ExchangeRatePreviewer.Core.Interfaces.Services;
using ExchangeRatePreviewer.Core.Interfaces.Services.Validators;
using ExchangeRatePreviewer.Core.Models.Dtos.ExchangeRate;
using ExchangeRatePreviewer.Core.Services.SOAP;

namespace ExchangeRatePreviewer.Core.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly IDateValidation _dateValidation;
    private readonly IBankOfLithuaniaSoapClient _client;

    public ExchangeRateService(IDateValidation dateValidation, IBankOfLithuaniaSoapClient client)
    {
        _dateValidation = dateValidation;
        _client = client;
    }

    public async Task<List<ExchangeRateDto>?> GetAllExchangeRatesByDate(DateTime date/*,
        BankOfLithuaniaSoapClientSettings clientSettings*/)
    {
        _dateValidation.IsDateValid(date);
        
        //var client = new BankOfLithuaniaSoapClient(clientSettings); 
        
        var selectedDateExchangeRates = await _client.GetExchangeRatesByDate(date);
        if (selectedDateExchangeRates is null)
            return null;
        
        var previousDayExchangeRates = await _client.GetExchangeRatesByDate(date.AddDays(-1));
        if (previousDayExchangeRates is null)
            return null;

        var exchangeRatesWithRateChange = GetExchangeRatesChangeOverPreviousDay(
            selectedDateExchangeRates.ExchangeRateDtos,
            previousDayExchangeRates.ExchangeRateDtos);
        var sortedExchangeRatesList = exchangeRatesWithRateChange.OrderByDescending(xrd => xrd.RateChangeVsPreviousDay).ToList();

        return sortedExchangeRatesList;
    }

    private static IEnumerable<ExchangeRateDto> GetExchangeRatesChangeOverPreviousDay(List<ExchangeRateDto> selectedDateExchangeRates, List<ExchangeRateDto> previousDayExchangeRates)
    {
        foreach (var exchangeRateDto in selectedDateExchangeRates)
        {
            var matchingExchangeRate =
                previousDayExchangeRates.FirstOrDefault(erd => erd.Currency == exchangeRateDto.Currency);

            if (matchingExchangeRate is not null)
                exchangeRateDto.RateChangeVsPreviousDay = exchangeRateDto.Rate - matchingExchangeRate.Rate;
            else
                exchangeRateDto.RateChangeVsPreviousDay = 0M;
        }

        return selectedDateExchangeRates;
    }
}