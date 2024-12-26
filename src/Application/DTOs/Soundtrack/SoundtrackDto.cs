namespace Application.DTOs.Soundtrack;

public record SoundtrackDto(
    Guid Id,
    string Title,
    double LengthInSeconds,
    string AlbumCoverFileName,
    Guid NextTrackId,
    Guid PrevTrackId);