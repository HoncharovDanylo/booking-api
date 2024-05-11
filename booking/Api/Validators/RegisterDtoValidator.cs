using booking_api.Context;
using booking_api.Models.DTOs.AccountDtos;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Api.Validators;

// Using Fluent Validation to validate the RegisterDto
public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator(BookingDbContext dbContext)
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(20).MustAsync( async (username,cancellation) => 
            !await dbContext.Users.AnyAsync(user => user.Username == username!)).WithMessage("Username already exists.");
        RuleFor(x => x.Password).NotEmpty().MaximumLength(50)
            .NotEqual(x=>x.Username)
            .NotEqual(x=>x.Email)
            .NotEqual(x=>x.Name);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress().MustAsync(async (email, cancellation) =>
            !await dbContext.Users.AnyAsync(user => user.Email == email!)).WithMessage("This email already used.");
    }
}