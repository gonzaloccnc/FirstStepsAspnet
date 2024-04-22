namespace FirstStepsAspnet.Models
{
  public class TodoItem
  {
    public long Id { get; set; }
    public string? Name { get; set; } // is optionnal in the db
    public bool IsComplete { get; set; }
    public string? Secret { get; set; }
    public long UserId { get; set; }
    public virtual User User { get; set; } = null!; // this can be deleted is equal that property above
  }
}
