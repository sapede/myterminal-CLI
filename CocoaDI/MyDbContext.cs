using CocoaDI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class MyDbContext : DbContext
{
    private readonly string _connectionString;

    public MyDbContext(IOptions<MyConfig> option) 
    {
        _connectionString = option.Value.SqliteConnectionString;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connectionString);
    }
}

