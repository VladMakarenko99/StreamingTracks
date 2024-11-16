using Application.DTOs.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Soundtracks.Queries.GetImage;

public record GetImageQuery(string FileName) : IRequest<Result<byte[]>>;