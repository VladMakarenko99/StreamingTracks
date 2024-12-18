using Application.DTOs.Result;
using Domain.Entities;
using MediatR;

namespace Application.Soundtracks.Queries.GetById;

public record GetByIdQuery(Guid id) :  IRequest<Result<Soundtrack>>;