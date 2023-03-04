using ExchangeRatePreviewer.Core.Services.SOAP;

namespace ExchangeRatePreviewer.Core.Interfaces.Services;

public interface IBankOfLithuaniaSoapClient
{
    Task<BankOfLithuaniaExchangeRateServiceResponse?> GetExchangeRatesByDate(DateTime date);
}