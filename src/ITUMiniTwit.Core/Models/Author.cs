using Microsoft.AspNetCore.Identity;
namespace ITUMiniTwit.Core.Models;

public class Author: IdentityUser
{
    public List<Cheep> Cheeps { get; set; } = new();
    public ICollection<Author> Following { get; set; } = new List<Author>();
    public ICollection<Author> Followers { get; set; } = new List<Author>();
}