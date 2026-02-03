using Xunit;
using Microsoft.Data.Sqlite;
using ITUMiniTwit.Infrastructure;
using ITUMiniTwit.Core.Models;
using ITUMiniTwit.Infrastructure.ITUMiniTwit.Repositories;
using Microsoft.EntityFrameworkCore;
using ITUMiniTwit.Core;
namespace ITUMiniTwit.InfrastructureTests;

public class DatabaseTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<ITUMiniTwitDBContext> _contextOptions;
    public DatabaseTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        _contextOptions = new DbContextOptionsBuilder<ITUMiniTwitDBContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new ITUMiniTwitDBContext(_contextOptions);

        context.Database.EnsureCreated();

        var bob = new Author { UserName = "Bob", Email = "bob@email.dk" };
        var ib = new Author { UserName = "Ib", Email = "ib@email.dk" };

        context.Authors.AddRange(bob, ib);
        context.Cheeps.AddRange(
            new Cheep { Text = "Hello", TimeStamp = DateTime.UtcNow, Author = bob },
            new Cheep { Text = "Halloween", TimeStamp = DateTime.UtcNow, Author = ib }
        );
        context.SaveChanges();
    }
    ITUMiniTwitDBContext CreateContext() => new ITUMiniTwitDBContext(_contextOptions);
    public void Dispose() => _connection.Dispose();

    [Fact]
    public void CheepRepository_Uses_SQLiteInMemooryDatabase()
    {
        // Arrange
        using var context = CreateContext();
        var cheepRepository = new CheepRepository(context);
  
        // Act
        var list = cheepRepository.GetCheeps(1, 10);

        // Assert
        Assert.Contains(list, c => c.Text == "Hello" && c.Author == "Bob");
        Assert.Contains(list, c => c.Text == "Halloween" && c.Author == "Ib");
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public void AuthorRepository_Can_GetAuthorByName()
    {
        // Arrange
        using var context = CreateContext();
        var authorRepository = new AuthorRepository(context);

        // Act
        var author = authorRepository.GetAuthorByName("Bob");

        // Assert
        Assert.Equal("Bob", author.UserName);
    }

    [Fact]
    public void AuthorRepository_Can_GetAuthorByEmail()
    {
        
        // Arrange
        using var context = CreateContext();
        var authorRepository = new AuthorRepository(context);

        // Act
        var author = authorRepository.GetAuthorByEmail("bob@email.dk");

        // Assert
        Assert.Equal("Bob", author.UserName);
    }

    [Fact]
    public void AuthorRepository_Can_AddAuthor()
    {
        //Arrange
        using var context = CreateContext();
        var authorRepository = new AuthorRepository(context);
        var name = "John";
        var email = "john@email.dk";

        //Act
        authorRepository.AddAuthor(name, email);
        var author = authorRepository.GetAuthorByName(name);

        //Assert
        Assert.Equal(email, author.Email);
        Assert.Equal(name, author.UserName);
    }

    [Fact]
    public void CheepRepository_Can_AddCheep()
    {
        //Arrange
        using var context = CreateContext();
        var cheepRepository = new CheepRepository(context);
        var cheepDto = new CheepDto
        {
            Text = "New Cheep",
            Author = "Bob",
            TimeStamp = DateTime.UtcNow.ToString()
        };

        //Act
        cheepRepository.AddCheep(cheepDto);
        var cheeps = cheepRepository.GetCheeps(1, 10);

        //Assert
        Assert.Contains(cheeps, c=> c.Text == "New Cheep" && c.Author == "Bob");
        Assert.Equal(3, cheeps.Count);
    }
}