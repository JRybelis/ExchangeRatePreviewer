using ExchangeRatePreviewer.Core.Services.SOAP;
using Microsoft.Extensions.Configuration;

namespace ExchangeRatePreviewer.Core.Interfaces.Services;

public interface IBankOfLithuaniaSoapClientSettings
{
    string? BaseUrl { get; }
    string RequestUrl { get; }
    string Host { get; }
    string ResultXmlNamespace { get; }
    //BankOfLithuaniaSoapClientSettings void GetClientSettings(IConfiguration configuration);
}