using ExchangeRatePreviewer.Core.Interfaces;
using ExchangeRatePreviewer.Core.Interfaces.Services;
using ExchangeRatePreviewer.Core.Models.Dtos.ExchangeRate;
using ExchangeRatePreviewer.Core.Services.SOAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ExchangeRatePreviewer.API.Controllers;

/// <summary>
/// Handles API requests for LTL currency exchange rates from bank of Lithuania
/// </summary>
[Route("lb/exchangeRates")]
public class ExchangeRateController : ControllerBase
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly IBankOfLithuaniaSoapClientSettings _clientSettings;

    //private readonly IOptions<BankOfLithuaniaSoapClientSettings> _bankOfLithuaniaSoapClientSettings;
    public ExchangeRateController(IExchangeRateService exchangeRateService,
        IBankOfLithuaniaSoapClientSettings
            clientSettings /*, IOptions<BankOfLithuaniaSoapClientSettings> bankOfLithuaniaSoapClientSettings*/)
    {
        _exchangeRateService = exchangeRateService;
        _clientSettings = clientSettings;
        //_bankOfLithuaniaSoapClientSettings = bankOfLithuaniaSoapClientSettings;
    }

    [HttpGet, /*ValidateFluent*/]
    public async Task<ActionResult<List<ExchangeRateDto>>> GetAll(DateTime date)
    {
        /*var bankOfLithuaniaSoapClientSettings = new BankOfLithuaniaSoapClientSettings
        {
            BaseUrl = _clientSettings.BaseUrl,
            RequestUrl = _clientSettings.RequestUrl,
            TimeoutMs = _clientSettings.TimeoutMs,
            ResultXmlNamespace = _clientSettings.ResultXmlNamespace
        };*/

        var result = await _exchangeRateService.GetAllExchangeRatesByDate(date/*, bankOfLithuaniaSoapClientSettings*/);
        
        if (result is null)
            return NotFound();

        return result;
    }
}