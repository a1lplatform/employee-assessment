using A1.SAS.Api.Dtos;
using A1.SAS.Infrastructure.Wrappers;

namespace A1.SAS.Api.Services
{
    public interface IRangeService
    {
        Task<Result<bool>> AddRangeAsync(List<RangeDto> rangesList);

        Task<Result<bool>> UpdateRangeAsync(List<RangeDto> rangesList);

        Task<Result<IEnumerable<RangeDto>>> GetRangesAsync();

        Task<Result<bool>> DeleteRangeAsync(List<Guid> ids);
    }
}
