using BreadBox_API.Models;
using FluentValidation;

namespace BreadBox_API.Validators
{
    public class InvoiceCreateValidator : AbstractValidator<InvoiceCreateModel>
    {
        public InvoiceCreateValidator()
        {
            RuleFor(model => model.ClientId)
                .GreaterThan(0).WithMessage("ClientId must be a positive integer.");

            RuleFor(model => model.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(model => model.IssueDate)
                .NotEmpty().WithMessage("Issue date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Issue date cannot be in the future.");

            RuleFor(model => model.DueDate)
                .NotEmpty().WithMessage("Due date is required.")
                .GreaterThan(model => model.IssueDate).WithMessage("Due date must be after issue date.");

            RuleFor(model => model.Status)
                .NotEmpty().WithMessage("Status is required.")
                .MaximumLength(50).WithMessage("Status must not exceed 50 characters.");

            RuleFor(model => model.UserId)
                .GreaterThan(0).WithMessage("UserId must be a positive integer.");
        }
    }
}