using Application.DTOs.Result;
using MediatR;

namespace Application.Soundtracks.Commands.IncreaseListeningCount;

public record IncreaseListeningCountCommand(Guid Id) : IRequest<Result<string>>;