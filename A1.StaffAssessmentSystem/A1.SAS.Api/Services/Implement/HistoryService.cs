
using A1.SAS.Api.Dtos;
using A1.SAS.Domain.Entities;
using A1.SAS.Domain.UnitOfWork;
using A1.SAS.Infrastructure.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace A1.SAS.Api.Services.Implement
{
    public class HistoryService : IHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public HistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<HistoryDto>>> GetHistoryAsync()
        {
            var historyDto = await (_unitOfWork.GetRepository<TblHistory>()
                .GetAll()
                .Where(x => !x.IsDeleted)
                .Select(x => new HistoryDto {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    SearchContent = x.SearchContent,
                    CreatedByName = _unitOfWork.GetRepository<TblAccount>().Get(x.CreatedBy) == null ? string.Empty : _unitOfWork.GetRepository<TblAccount>().Get(x.CreatedBy).Username
                })).ToListAsync();

            return await Result<IEnumerable<HistoryDto>>.SuccessAsync(historyDto);
        }

        public async Task<Result<IEnumerable<HistoryDto>>> GetHistoryByAccountAsync(Guid accountId)
        {
            var historyDto = await (_unitOfWork.GetRepository<TblHistory>()
               .GetAll()
               .Where(x => !x.IsDeleted && x.CreatedBy == accountId.ToString())
               .Select(x => new HistoryDto
               {
                   Id = x.Id,
                   CreatedAt = x.CreatedAt,
                   CreatedBy = x.CreatedBy,
                   SearchContent = x.SearchContent,
                   CreatedByName = _unitOfWork.GetRepository<TblAccount>().Get(x.CreatedBy) == null ? string.Empty : _unitOfWork.GetRepository<TblAccount>().Get(x.CreatedBy).Username
               })).ToListAsync();

            return await Result<IEnumerable<HistoryDto>>.SuccessAsync(historyDto);
        }
    }
}
