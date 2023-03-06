namespace ExchangeRatePreviewer.Core.Interfaces.Services.SOAP;

public interface IBankOfLithuaniaSoapClientSettings
{
    string? BaseUrl { get; }
    string RequestUrl { get; set;  }
    string Host { get; }
}