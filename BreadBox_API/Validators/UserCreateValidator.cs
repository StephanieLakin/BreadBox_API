using BreadBox_API.Models;
using FluentValidation;

namespace BreadBox_API.Validators
{
    public class UserCreateValidator : AbstractValidator<UserCreateModel>
    {
        public UserCreateValidator()
        {
            RuleFor(model => model.EmailAddress)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("Invalid email address format.")
                .MaximumLength(255).WithMessage("Email address must not exceed 255 characters.");

            RuleFor(model => model.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.");

            RuleFor(model => model.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

            RuleFor(model => model.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

            RuleFor(model => model.SubscriptionPlan)
                .NotEmpty().WithMessage("Subscription plan is required.")
                .MaximumLength(50).WithMessage("Subscription plan must not exceed 50 characters.");

            RuleFor(model => model.StripeCustomerId)
                .NotEmpty().WithMessage("Stripe customer ID is required.")
                .MaximumLength(50).WithMessage("Stripe customer ID must not exceed 50 characters.");
        }
    }
}
