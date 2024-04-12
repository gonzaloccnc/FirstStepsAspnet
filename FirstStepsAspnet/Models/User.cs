using System.ComponentModel.DataAnnotations;

namespace FirstStepsAspnet.Models
{
  public class User
  {
    public long Id { get; set; }

    [MaxLength(20)]
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    public List<TodoItem> TodoItems { get; set; } = [];
  }
}
