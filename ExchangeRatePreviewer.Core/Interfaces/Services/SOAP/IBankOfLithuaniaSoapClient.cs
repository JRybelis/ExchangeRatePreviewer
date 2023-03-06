using ExchangeRatePreviewer.Core.Models.Dtos.ExchangeRate;

namespace ExchangeRatePreviewer.Core.Interfaces.Services.SOAP;

public interface IBankOfLithuaniaSoapClient
{
    Task<List<ExchangeRateDto>?> GetExchangeRatesByDate(DateTime date);
}