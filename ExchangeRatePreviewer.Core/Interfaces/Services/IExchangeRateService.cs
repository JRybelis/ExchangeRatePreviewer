using ExchangeRatePreviewer.Core.Models.Dtos.ExchangeRate;
using ExchangeRatePreviewer.Core.Services.SOAP;

namespace ExchangeRatePreviewer.Core.Interfaces.Services;

public interface IExchangeRateService
{
    Task<List<ExchangeRateDto>?> GetAllExchangeRatesByDate(DateTime date/*, BankOfLithuaniaSoapClientSettings clientSettings*/);
}