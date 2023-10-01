using Api.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.UseCases
{
    public class GetTopMakelaars : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/makelaars/top", ([FromQuery] string[] filters, [FromQuery] int total, IMediator mediator) =>
            {
                return mediator.Send(new GetTopMakelaarsQuery(filters.ToList(), total));
            });
        }

        public class GetTopMakelaarsQuery : IRequest<GetTopMakelaarsResponse> {
            public List<string> Filters { get; }
            public int Total { get; }

            public GetTopMakelaarsQuery(List<string> filters, int total)
            {
                Filters = filters;
                Total = total;
            }
        }

        public class GetTopMakelaarsHandler : IRequestHandler<GetTopMakelaarsQuery, GetTopMakelaarsResponse>
        {
            private readonly ITopMakelaarsRepository _repository;

            public GetTopMakelaarsHandler(ITopMakelaarsRepository repository)
            {
                _repository = repository;
            }

            public Task<GetTopMakelaarsResponse> Handle(GetTopMakelaarsQuery request, CancellationToken cancellationToken) =>
               _repository.GetTopMakelaarsAsync(request.Filters, request.Total);
        }

        public record GetTopMakelaarsResponse(List<TopMakelaar> Makelaars, bool IncompleteData);
        public record TopMakelaar(int Position, string Name, int TotalHouses);
    }
}
