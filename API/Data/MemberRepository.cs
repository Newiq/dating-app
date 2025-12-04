using System;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class MemberRepository(AppDbContext context) : IMemberRepository
{
    public async Task<Member?> GetMemberByIdAsync(string id)
    {
        return await context.Members
        .FindAsync(id);
    }

#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
    public async Task<Member?> GetMemberForUpdate(string id)
#pragma warning restore CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
    {
        return await context.Members
        .Include(x => x.User)
        .Include(x => x.Photos)
        .SingleOrDefaultAsync( x => x.Id == id);
    }

    public async Task<IReadOnlyList<Member>> GetMembersAllAsync()
    {
        return await context.Members
        .ToListAsync();
    }

    public async Task<IReadOnlyList<Photo>> GetPhotosAsync(string memberId)
    {
        return await context.Members.Where(x => x.Id == memberId)
            .SelectMany(x => x.Photos)
            .ToListAsync(); 
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Update(Member member)
    {
        context.Entry(member).State = EntityState.Modified;
    }
}
