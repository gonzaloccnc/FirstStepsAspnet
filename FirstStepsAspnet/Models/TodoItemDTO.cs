namespace FirstStepsAspnet.Models
{
  public class TodoItemDTO
  {
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public long UserId { get; set; }
  }
}
