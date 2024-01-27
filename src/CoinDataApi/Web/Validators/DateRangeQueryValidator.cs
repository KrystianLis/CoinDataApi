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
    }

    private static bool BeAValidDateAndUtc(string value)
    {
        return DateTime.TryParse(value, null, DateTimeStyles.AdjustToUniversal, out var dateTime)
               && dateTime.Kind == DateTimeKind.Utc;
    }
}