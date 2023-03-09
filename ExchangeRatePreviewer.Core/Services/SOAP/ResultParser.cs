using System.Globalization;
using System.Xml;
using ExchangeRatePreviewer.Core.Interfaces.Services.SOAP;
using ExchangeRatePreviewer.Core.Models.Dtos.ExchangeRate;
using Microsoft.Extensions.Logging;

namespace ExchangeRatePreviewer.Core.Services.SOAP;

public class ResultParser : IResultParser
{
    private readonly ILogger<ResultParser> _logger;
    public ResultParser(ILogger<ResultParser> logger)
    {
        _logger = logger;
    }

    public List<ExchangeRateDto> ParseResponseXmlElement(string responseString)
    {
        var response = new List<ExchangeRateDto>();
        var xmlDocument = new XmlDocument();

        xmlDocument.LoadXml(responseString);
        if (xmlDocument.DocumentElement == null) return response;

        var exchangeRates = xmlDocument.DocumentElement.SelectNodes("/ExchangeRates");
        var exchangeRatesItems = exchangeRates.Item(0).ChildNodes;

        try
        {
            // Populate BankOfLithuaniaExchangeRateServiceResponse with parsed values
            foreach (XmlElement exchangeRatesItem in exchangeRatesItems)
                response.Add(PopulateExchangeRateDto(exchangeRatesItem));
        }
        catch (Exception ex)
        {
            _logger.LogDebug("{ExInnerException} caused {ExMessage}", ex.InnerException, ex.Message);
            throw;
        }

        return response;
    }
    
    private static ExchangeRateDto PopulateExchangeRateDto(XmlNode exchangeRatesItem)
    {
        var exchangeRateDto = new ExchangeRateDto();

        foreach (XmlElement element in exchangeRatesItem.ChildNodes)
        {
            switch (element.LocalName)
            {
                case "date":
                    exchangeRateDto.Date = DateTime.Parse(element.InnerText);
                    break;
                case "currency":
                    exchangeRateDto.Currency = element.InnerText;
                    break;
                case "quantity":
                    exchangeRateDto.Quantity = int.Parse(element.InnerText);
                    break;
                case "rate":
                    exchangeRateDto.Rate = decimal.TryParse(element.InnerText,
                        NumberStyles.Currency,
                        CultureInfo.InvariantCulture,
                        out var rate)
                        ? rate
                        : decimal.Zero;
                    break;
                case "unit":
                    exchangeRateDto.UnitDescription = element.InnerText;
                    break;
            }
        }

        return exchangeRateDto;
    }
}