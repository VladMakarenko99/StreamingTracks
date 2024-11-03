using Application.DTOs.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Soundtracks.Commands.Upload;

public record UploadFileCommand(IFormFile File) : IRequest<Result<string>>;