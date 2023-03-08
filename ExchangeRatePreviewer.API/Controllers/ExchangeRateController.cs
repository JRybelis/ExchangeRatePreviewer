using ExchangeRatePreviewer.Core.Interfaces.Services;
using ExchangeRatePreviewer.Core.Models.Dtos.ExchangeRate;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRatePreviewer.API.Controllers;

/// <summary>
/// Handles API requests for LTL currency exchange rates from bank of Lithuania
/// </summary>
[Route("lb/exchangeRates")]
public class ExchangeRateController : ControllerBase
{
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeRateController(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ExchangeRateDto>>> GetByDate(DateTime date)
    {
        var result = await _exchangeRateService.GetAllExchangeRatesByDate(date);
        
        if (result is null)
            return NotFound();

        return result;
    }
}