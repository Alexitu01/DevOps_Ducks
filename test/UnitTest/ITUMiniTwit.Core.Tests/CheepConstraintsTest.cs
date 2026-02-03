using System;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using ITUMiniTwit.Core.Models;
using ITUMiniTwit.Infrastructure.ITUMiniTwit.Repositories;
using ITUMiniTwit.Core;
using ITUMiniTwit.Infrastructure;

namespace ITUMiniTwit.Core.Tests
{
    public class CheepConstraintsTest
    {
        private readonly CheepRepository _cheepRepository;
        private readonly AuthorRepository _authorRepository;
        private readonly ITUMiniTwitDBContext _context;

        public CheepConstraintsTest()
        {
            var options = new DbContextOptionsBuilder<ITUMiniTwitDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ITUMiniTwitDBContext(options);
            _authorRepository = new AuthorRepository(_context);
            _cheepRepository = new CheepRepository(_context);
            _authorRepository.AddAuthor("Daid","daid@email.dk");
        }

        [Fact]
        public void Cheep_160_Characters()
        {
            //Arrange
            var cheepDto = new CheepDto
            {
                Text = new string('a', 160),
                Author = "Daid",
                TimeStamp = DateTime.Now.ToString()
            };

            //Act
            _cheepRepository.AddCheep(cheepDto);
            var result = _cheepRepository.GetCheeps(1, 10);

            //Assert
            Assert.Contains(result, c => c.Text == cheepDto.Text && c.Author == cheepDto.Author);
        }

        [Fact]
        public void Cheep_161_Characters()
        {
            //Arrange
            var cheepDto = new CheepDto
            {
                Text = new string('a', 161),
                Author = "Daid",
                TimeStamp = DateTime.Now.ToString()
            };

            //Act & Assert
            Assert.Throws<ArgumentException>(() => _cheepRepository.AddCheep(cheepDto));
        }
    }
}