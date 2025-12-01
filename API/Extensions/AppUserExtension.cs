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
            UserName = user.UserName,
            ImageUrl = user.ImageUrl,
            Token = tokenServices.CreateToken(user)
        };
    }
}
