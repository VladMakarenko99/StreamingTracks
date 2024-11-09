using Application.DTOs.Result;
using MediatR;

namespace Application.Soundtracks.Commands.Delete;

public record DeleteFileCommand(Guid Id) : IRequest<Result<string>>;