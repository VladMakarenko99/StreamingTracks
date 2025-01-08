using Application.DTOs.Result;
using Application.DTOs.Soundtrack;
using Domain.Entities;
using MediatR;

namespace Application.Soundtracks.Queries.GetById;

public record GetBySlugQuery(string Slug) :  IRequest<Result<SoundtrackDto>>;