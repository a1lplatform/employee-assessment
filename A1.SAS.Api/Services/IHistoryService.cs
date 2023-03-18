using A1.SAS.Api.Dtos;
using A1.SAS.Infrastructure.Wrappers;

namespace A1.SAS.Api.Services
{
    public interface IHistoryService
    {
        Task<Result<IEnumerable<HistoryDto>>> GetHistoryAsync();
        Task<Result<IEnumerable<HistoryDto>>> GetHistoryByAccountAsync(Guid accountId);
    }
}
