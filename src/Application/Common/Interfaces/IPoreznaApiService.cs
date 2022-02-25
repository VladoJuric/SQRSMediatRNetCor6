using Application.Dtos;

namespace Application.Common.Interfaces
{
    public interface IPoreznaApiService
    {
        Task<List<PoreznaDto>> GetPorez(CancellationToken cancellationToken);
    }
}
