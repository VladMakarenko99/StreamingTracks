namespace Domain.Entities;

public class Soundtrack
{
    public Guid Id { get; set; }
    
    public string  Title { get; set; }

    public string Extension { get; set; }

    public double LengthInSeconds { get; set; }
    
    public string AlbumCoverFileName { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string? Slug { get; set; }
}