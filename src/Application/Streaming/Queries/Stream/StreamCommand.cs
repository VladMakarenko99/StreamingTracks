using Application.DTOs.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Streaming.Queries.Stream;

public record StreamCommand(Guid Id) : IRequest<Result<FileStreamResult>>;