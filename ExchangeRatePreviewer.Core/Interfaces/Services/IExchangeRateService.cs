using ExchangeRatePreviewer.Core.Models.Dtos.ExchangeRate;

namespace ExchangeRatePreviewer.Core.Interfaces.Services;

public interface IExchangeRateService
{
    Task<List<ExchangeRateDto>?> GetAllExchangeRatesByDate(DateTime date);
}