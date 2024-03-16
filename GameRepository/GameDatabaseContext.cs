using System;
using GameRepository.Entity;
using Microsoft.EntityFrameworkCore;

namespace GameRepository
{
    public class GameDatabaseContext : DbContext
    {
        public GameDatabaseContext(DbContextOptions<GameDatabaseContext> options)
            : base(options)
        {
        }

        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.Entity is BaseEntity trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedDateTime = utcNow;
                            entry.Property("CreatedDateTime").IsModified = false;
                            break;
                        case EntityState.Added:
                            trackable.CreatedDateTime = utcNow;
                            trackable.UpdatedDateTime = utcNow;
                            break;
                    }
                }
            }
        }

        public DbSet<UserInfo> UserInfo { get; set; }

        public DbSet<ScoreBoard> ScoreBoard { get; set; }

        public DbSet<UserGame> UserGame { get; set; }
    }
}