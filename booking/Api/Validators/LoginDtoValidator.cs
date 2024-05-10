using booking_api.Models.DTOs.AccountDtos;
using FluentValidation;

namespace Api.Validators;
// Using Fluent Validation to validate the LoginDto
public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(50);
    }
}