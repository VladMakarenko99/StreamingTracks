namespace Application.Abstractions;

public interface ICacheService
{
    Task RefreshSoundtracksAsync(CancellationToken cancellationToken);
}