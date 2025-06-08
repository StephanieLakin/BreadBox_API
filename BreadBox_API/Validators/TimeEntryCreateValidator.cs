using BreadBox_API.Models;
using FluentValidation;

namespace BreadBox_API.Validators
{
    public class TimeEntryCreateValidator : AbstractValidator<TimeEntryCreateModel>
    {
        public TimeEntryCreateValidator()
        {
            RuleFor(model => model.ClientId)
                .GreaterThan(0).WithMessage("ClientId must be a positive integer.");

            RuleFor(model => model.StartTime)
                .NotEmpty().WithMessage("Start time is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Start time cannot be in the future.");

            RuleFor(model => model.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(model => model.StartTime).WithMessage("End time must be after start time.");

            RuleFor(model => model.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(model => model.Rate)
                .GreaterThanOrEqualTo(0).WithMessage("Rate cannot be negative.");

            RuleFor(model => model.UserId)
                .GreaterThan(0).WithMessage("UserId must be a positive integer.");
        }
    }
}