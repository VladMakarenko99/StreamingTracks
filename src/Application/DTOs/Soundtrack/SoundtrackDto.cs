namespace Application.DTOs.Soundtrack;

public record SoundtrackDto(
    Guid Id,
    string Title,
    double LengthInSeconds,
    string Slug,
    string NextTrackSlug,
    string PrevTrackSlug,
    string MusicFileUrl,
    string AlbumCoverUrl);