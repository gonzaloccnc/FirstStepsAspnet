using System.ComponentModel.DataAnnotations;

namespace FirstStepsAspnet.Models
{
  public class TodoItemDTO
  {
    public long Id { get; set; }

    [Required]
    [StringLength(50)]
    [MinLength(10)]
    public string? Name { get; set; }

    [Required]
    public bool IsComplete { get; set; }

    [Required]
    public long UserId { get; set; }
  }
}
