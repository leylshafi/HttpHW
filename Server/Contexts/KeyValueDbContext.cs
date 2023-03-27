using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Model;

namespace Server.Contexts;

public class KeyValueDbContext:DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

        var connectionString = configuration.GetConnectionString("Db");

        optionsBuilder.UseSqlServer(connectionString);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KeyValue>().HasKey(e => e.Key);
        base.OnModelCreating(modelBuilder);
    }

    DbSet<KeyValue> KeyValues { get; set; }
}
