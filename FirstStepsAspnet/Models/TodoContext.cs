using Microsoft.EntityFrameworkCore;

namespace FirstStepsAspnet.Models
{
  public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
  {
    public DbSet<TodoItem> TodoItems { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //  modelBuilder.Entity<User>().Property(b => b.Username).IsRequired();
    //}

  }
}
