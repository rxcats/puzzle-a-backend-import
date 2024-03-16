using System;
using GameRepository;
using Microsoft.EntityFrameworkCore;

namespace GameRepositoryTest
{
    public static class TestDbOptions
    {
        public static DbContextOptions<GameDatabaseContext> Create()
        {
            return new DbContextOptionsBuilder<GameDatabaseContext>()
                .UseMySql(@"Server=localhost;Uid=root;Pwd=qwer1234;Database=puzzle_a;", new MySqlServerVersion("5.7.0"))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .LogTo(Console.WriteLine)
                .Options;
        }
    }
}