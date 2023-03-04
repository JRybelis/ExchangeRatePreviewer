using ExchangeRatePreviewer.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace ExchangeRatePreviewer.Core.Services.SOAP;

public class BankOfLithuaniaSoapClientSettings : IBankOfLithuaniaSoapClientSettings
{
    private readonly IConfiguration _configuration;

    public string? BaseUrl { get; set; }
    public string RequestUrl { get; set; }
    public string Host { get; set; }
    public string ResultXmlNamespace { get; set; }
}