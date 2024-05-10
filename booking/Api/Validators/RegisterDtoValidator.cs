using booking_api.Models.DTOs.AccountDtos;
using FluentValidation;

namespace Api.Validators;

// Using Fluent Validation to validate the RegisterDto
public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(50)
            .NotEqual(x=>x.Username)
            .NotEqual(x=>x.Email)
            .NotEqual(x=>x.Name);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
    }
}