using System.Globalization;
using CoinDataApi.Web.Query;
using FluentValidation;

namespace CoinDataApi.Web.Validators;

public class DateRangeQueryValidator : AbstractValidator<DateRangeQuery>
{
    public DateRangeQueryValidator()
    {
        RuleFor(query => query.TimeStart)
            .NotEmpty()
            .WithMessage("TimeStart cannot be empty.")
            .Must(BeAValidDateAndUtc!)
            .WithMessage("Invalid TimeStart format. Please use ISO 8601 format in UTC.");

        RuleFor(query => query.TimeEnd)
            .NotEmpty()
            .WithMessage("TimeEnd cannot be empty.")
            .Must(BeAValidDateAndUtc!)
            .WithMessage("Invalid TimeEnd format. Please use ISO 8601 format in UTC.")
            .DependentRules(() =>
            {
                RuleFor(query => query)
                    .Must(query => DateTime.Parse(query.TimeEnd!) >= DateTime.Parse(query.TimeStart!))
                    .WithMessage("TimeEnd must not be earlier than TimeStart.")
                    .WithName("Message");
            });

        RuleFor(query => query.PeriodId)
            .NotEmpty()
            .WithMessage("PeriodId cannot be empty.")
            .Must(BeAValidPeriodId!)
            .WithMessage("Invalid PeriodId. Please use one of the specified formats (e.g., 1SEC, 2MIN, 1HRS).");
    }

    private static bool BeAValidDateAndUtc(string value)
    {
        return DateTime.TryParse(value, null, DateTimeStyles.AdjustToUniversal, out var dateTime)
               && dateTime.Kind == DateTimeKind.Utc;
    }
    
    private static bool BeAValidPeriodId(string periodId)
    {
        var validPeriodIds = new[]
        {
            "1SEC", "2SEC", "3SEC", "4SEC", "5SEC", "6SEC", "10SEC", "15SEC", "20SEC", "30SEC",
            "1MIN", "2MIN", "3MIN", "4MIN", "5MIN", "6MIN", "10MIN", "15MIN", "20MIN", "30MIN",
            "1HRS", "2HRS", "3HRS", "4HRS", "6HRS", "8HRS", "12HRS",
            "1DAY", "2DAY", "3DAY", "5DAY", "7DAY", "10DAY",
            "1MTH", "2MTH", "3MTH", "4MTH", "6MTH",
            "1YRS", "2YRS", "3YRS", "4YRS", "5YRS"
        };

        return validPeriodIds.Contains(periodId);
    }
}