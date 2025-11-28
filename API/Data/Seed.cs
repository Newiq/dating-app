using System;
using System.Security.Cryptography;
using System.Text.Json;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers (AppDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var users = JsonSerializer.Deserialize<List<SeedUserDto>>(userData);

        if (users == null)
        {
            Console.WriteLine("No users in seed data");
            return;
        }

       

        foreach (var user in users)
        {
            // Create password hash and salt
            using var hmac = new HMACSHA512();
            var password = "Pa$$w0rd";
            var appUser = new AppUser
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                ImageUrl = user.ImageUrl,
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key,
                Member = new Member
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Description = user.Description,
                    DateOfBirth = user.DateOfBirth,
                    ImageUrl = user.ImageUrl,
                    Gender = user.Gender,
                    City = user.City,
                    Country = user.Country,
                    Created = user.Created,
                    LastActive = user.LastActive
                }
            };

           
            appUser.Member.Photos.Add(new Photo
            {
                Url = user.ImageUrl!,
                MemberId = user.Id

            });

            context.Users.Add(appUser);
        }

        await context.SaveChangesAsync();
    }
}
