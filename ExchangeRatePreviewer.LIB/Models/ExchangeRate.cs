using ExchangeRatePreviewer.LIB.Enums;

namespace ExchangeRatePreviewer.LIB.Models;

public class ExchangeRate
{
    public Guid Id { get; set; }
    public Currency Currency { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public string? UnitDescription { get; set; }
    public decimal Rate { get; set; }
    public decimal RateChangeVsPreviousDay { get; set; }
}