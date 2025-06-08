using BreadBox_API.Models;
using FluentValidation;

namespace BreadBox_API.Validators
{
    public class ClientCreateValidator : AbstractValidator<ClientCreateModel>
    {
        public ClientCreateValidator()
        {
            RuleFor(model => model.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(model => model.EmailAddress)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("Invalid email address format.")
                .MaximumLength(255).WithMessage("Email address must not exceed 255 characters.");

            RuleFor(model => model.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{3}-\d{3}-\d{4}$").WithMessage("Phone number must be in the format XXX-XXX-XXXX.");

            RuleFor(model => model.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(500).WithMessage("Address must not exceed 500 characters.");

            RuleFor(model => model.UserId)
                .GreaterThan(0).WithMessage("UserId must be a positive integer.");
        }
    }
}
