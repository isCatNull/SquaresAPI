namespace Domain.Entities;

public class List
{
    public int Id { get; set; }
    public List<Point> Points { get; set; } = new List<Point>();
    public List<Square> Squares { get; set; } = new List<Square>();
}

