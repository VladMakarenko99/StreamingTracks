namespace Domain.Entities;

public class Soundtrack
{
    public Guid Id { get; set; }
    
    public string  Title { get; set; }

    public string Extension { get; set; }

    public double LengthInSeconds { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string Slug { get; set; }

    public string MusicFileUrl { get; set; }
    
    public string? AlbumCoverUrl { get; set; }
    
    public int? Listenings { get; set; }
}