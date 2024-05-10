using System.IdentityModel.Tokens.Jwt;
using System.Text;
using booking_api.Context;
using booking_api.Models;
using booking_api.Models.DTOs.AccountDtos;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly BookingDbContext _dbContext;
    private readonly IConfiguration _config;
    private readonly IValidator<RegisterDto> _registerValidator;
    private readonly IValidator<LoginDto> _loginValidator;

    public AccountController(BookingDbContext dbContext, IConfiguration config, IValidator<RegisterDto> registerValidator, IValidator<LoginDto> loginValidator)
    {
        _dbContext = dbContext;
        _config = config;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }
    
    [HttpPost("/api/register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var validation = await _registerValidator.ValidateAsync(registerDto);
        if (validation.IsValid)
        {
            //TODO: Add validation for unique username and email
            var user = new User
            {
                Username = registerDto.Username,
                Password = registerDto.Password,
                Name = registerDto.Name,
                Email = registerDto.Email
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            
            return Authenticate();
        }

        return BadRequest(validation.Errors);
    }
    [HttpPost("api/login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var validation = await _loginValidator.ValidateAsync(loginDto);
        if (validation.IsValid)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => (x.Username == loginDto.Username || x.Email == loginDto.Username) && x.Password == loginDto.Password);
            if (user != null)
                return Authenticate();
            return NotFound("Incorrect username or password");
        }
        return BadRequest(validation.Errors);
    }
    
    [Authorize]
    [HttpGet("api/test")]
    public IActionResult Test()
    {
        return Ok("You are authorized");
    }
    
    [NonAction]
    private IActionResult Authenticate()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var Sectoken = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            null,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
        return Ok(token);


    }
    
}