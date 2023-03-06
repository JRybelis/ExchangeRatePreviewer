using ExchangeRatePreviewer.Core.Interfaces.Services;
using ExchangeRatePreviewer.Core.Interfaces.Services.SOAP;

namespace ExchangeRatePreviewer.Core.Services.SOAP;

public class BankOfLithuaniaSoapClientSettings : IBankOfLithuaniaSoapClientSettings
{
    public string? BaseUrl { get; set; }
    public string RequestUrl { get; set; } = default!;
    public string Host { get; set; } = default!;
}