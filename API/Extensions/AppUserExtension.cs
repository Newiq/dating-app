using System;
using API.DTOs;
using API.Entities;
using API.Interfaces;
namespace API.Extensions;

public static class AppUserExtension
{
    public static UserDto ToDto(this AppUser user, ITokenServices tokenServices)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.UserName,
            Token = tokenServices.CreateToken(user)
        };
    }
}
