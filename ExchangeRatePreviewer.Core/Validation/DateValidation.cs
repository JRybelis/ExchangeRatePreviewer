using ExchangeRatePreviewer.Core.Interfaces.Services.Validators;

namespace ExchangeRatePreviewer.Core.Validation;

public class DateValidation : IDateValidation
{
    private const string MAXIMUM_ACCEPTED_QUERY_DATE =
        "This service only accepts requests for the period of up to the 31st of December, 2014.";

    private const string NO_DATE_PROVIDED =
        "This service requires a date to execute. Please supply a date, prior to the 31st of December, 2014.";

    private const string INCORRECT_DATE_FORMAT = "Unable to convert the date provided to a valid date.";

    private readonly DateTime _maxAcceptedDate = new DateTime(2014, 12, 31).Date;

    public void IsDateValid(DateTime dateTime)
    {
        var dateString = dateTime.ToString();

        if (string.IsNullOrWhiteSpace(dateString) || dateString == "0001-01-01 00:00:00")
            // No date, throw exception.
            throw new ArgumentNullException(NO_DATE_PROVIDED);

        if (!DateTime.TryParse(dateString, out var date))
            // Not a valid date, throw exception.
            throw new ArgumentException(INCORRECT_DATE_FORMAT);

        if (date > _maxAcceptedDate)
            // Above maximum accepted date, throw exception.
            throw new ArgumentOutOfRangeException(MAXIMUM_ACCEPTED_QUERY_DATE);
    }
}