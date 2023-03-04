namespace ExchangeRatePreviewer.Core.Models.Dtos.ExchangeRate;

public class ExchangeRateDto
{
    public Guid Id { get; set; }
    public string Currency { get; set; } = default!;
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public string UnitDescription { get; set; } = default!;
    public decimal Rate { get; set; }
    public decimal RateChangeVsPreviousDay { get; set; }
}

