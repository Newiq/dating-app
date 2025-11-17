using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController(AppDbContext context): BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<AppUser>> Register(string email, string username, string password)
    {
        //hash password
        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            Email = email,
            UserName = username,
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)),
            PasswordSalt = hmac.Key
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }
}
