using Application.Abstractions;
using Application.DTOs.Result;
using Domain.Entities;
using MediatR;

namespace Application.Soundtracks.Queries.GetAll;

public class GetAllQueryHandler(ISoundtrackRepository repository)
    : IRequestHandler<GetAllQuery, Result<List<Soundtrack>>>
{
    public async Task<Result<List<Soundtrack>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        return Result<List<Soundtrack>>.Success(await repository.GetAll());
    }
}