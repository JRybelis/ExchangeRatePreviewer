using ExchangeRatePreviewer.Core.Models.Dtos.ExchangeRate;

namespace ExchangeRatePreviewer.Core.Interfaces.Services.SOAP;

public interface IResultParser
{
    List<ExchangeRateDto> ParseResponseXmlElement(string responseString);
}