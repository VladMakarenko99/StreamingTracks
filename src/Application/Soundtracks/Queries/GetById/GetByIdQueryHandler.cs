using Application.Abstractions;
using Application.DTOs.Result;
using Domain.Entities;
using MediatR;
using Exception = System.Exception;

namespace Application.Soundtracks.Queries.GetById;

public class GetByIdQueryHandler(ISoundtrackRepository repository) : IRequestHandler<GetByIdQuery, Result<Soundtrack>>
{
    public async Task<Result<Soundtrack>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var soundtrack = await repository.GetById(request.id);
            return Result<Soundtrack>.Success(soundtrack);
        }
        catch (Exception e)
        {
            return Result<Soundtrack>.Failure($"{e.Message}");
        }
    }
}