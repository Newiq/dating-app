namespace API.Entities;
public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string UserName { get; set; }

    public required string Email { get; set; }

    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;

}