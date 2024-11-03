using Application.DTOs.Result;
using Domain.Entities;
using MediatR;

namespace Application.Soundtracks.Queries.GetAll;

public record GetAllQuery() : IRequest<Result<List<Soundtrack>>>;