using System.ComponentModel.DataAnnotations;
using ExchangeRatePreviewer.Core.Validation.DataAnnotations;

namespace ExchangeRatePreviewer.Core.Models.Dtos.ExchangeRate;

public class ExchangeRateDto
{
    [Required, MinLength(3), MaxLength(3)] public string Currency { get; set; } = default!;
    
    [Required, DataType(DataType.Date), CustomDateValidation] public DateTime Date { get; set; }
    
    [Required] public int Quantity { get; set; }

    [Required] public string UnitDescription { get; set; } = default!;
    
    [Required] public decimal Rate { get; set; }
}