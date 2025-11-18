using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.DTOs; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenServices tokenServices) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDTO registerDto)
    {
        // Debugging output
        Console.WriteLine($"RegisterDTO received: {registerDto?.UserName}, {registerDto?.Email}");
        
        if (registerDto == null)
        {
            Console.WriteLine("RegisterDTO is null - JSON binding failed");
            return BadRequest("Registration data is required");
        }

        // Model validation
        if (!ModelState.IsValid)
        {
            Console.WriteLine("ModelState is invalid");
            foreach (var error in ModelState)
            {
                Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
            }
            return BadRequest(ModelState);
        }

        // user exist checks
        if (await context.Users.AnyAsync(u => u.Email == registerDto.Email))
        {
            return BadRequest("Email is already taken");
        }

        if (await context.Users.AnyAsync(u => u.UserName == registerDto.UserName))
        {
            return BadRequest("Username is already taken");
        }

        // Hash password
        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            Email = registerDto.Email,
            UserName = registerDto.UserName,
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };
        
        context.Users.Add(user);
        await context.SaveChangesAsync();
        
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.UserName,
            Token = tokenServices.CreateToken(user)
        };
    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users
            .SingleOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null)
        {
            return Unauthorized("Invalid email address");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid password");
            }
        }

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.UserName,
            Token = tokenServices.CreateToken(user)
        };
    }
}