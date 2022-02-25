using Application.Common.Interfaces;
using Application.Dtos;
using MediatR;

namespace Application.Administrator.Queries
{
    public class GetPoreznaUpravaQuery : IRequest<List<PoreznaDto>>
    {
    }

    public class GetPoreznaUpravaQueryHandler : IRequestHandler<GetPoreznaUpravaQuery, List<PoreznaDto>>
    {
        private readonly IPoreznaApiService porezna;

        public GetPoreznaUpravaQueryHandler(IPoreznaApiService porezna)
        {
            this.porezna = porezna ?? throw new ArgumentNullException(nameof(porezna));
        }

        public async Task<List<PoreznaDto>> Handle(GetPoreznaUpravaQuery request, CancellationToken cancellationToken)
        {
            return await porezna.GetPorez(cancellationToken);
        }
    }
}
