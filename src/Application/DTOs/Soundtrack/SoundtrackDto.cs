namespace Application.DTOs.Soundtrack;

public record SoundtrackDto(
    Guid Id,
    string Title,
    double LengthInSeconds,
    string AlbumCoverFileName,
    string Slug,
    string NextTrackSlug,
    string PrevTrackSlug);