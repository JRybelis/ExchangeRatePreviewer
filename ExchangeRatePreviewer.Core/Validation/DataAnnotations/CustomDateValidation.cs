using System.ComponentModel.DataAnnotations;

namespace ExchangeRatePreviewer.Core.Validation.DataAnnotations;

public class CustomDateValidation : ValidationAttribute
{
    private const string MAXIMUM_ACCEPTED_QUERY_DATE =
        "This service only accepts requests for the period of up to the 31st of December, 2014.";

    private readonly DateTime _maxAcceptedDate = new DateTime(2014 - 12 - 31).Date;

    /// <summary>
    /// Checks whether the provided date is prior to the start of the year 2015.
    /// </summary>
    /// <param name="value">Input from the calling method</param>
    /// <param name="validationContext">Context for validation check</param>
    /// <returns>Boolean success or error and an error message, if any.</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var valueString = value?.ToString();

        if (string.IsNullOrWhiteSpace(valueString))
            // No value, so return success.
            return ValidationResult.Success;

        if (!DateTime.TryParse(valueString, out var date))
            // Not a valid date, return error.
            return new ValidationResult("Unable to convert the date provided to a valid date.");

        return date > _maxAcceptedDate ?
            // Above maximum accepted date, return error.
            new ValidationResult(MAXIMUM_ACCEPTED_QUERY_DATE) :
            // Return success
            ValidationResult.Success;
    }
}