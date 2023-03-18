
using A1.SAS.Api.Dtos;
using A1.SAS.Domain.Entities;
using A1.SAS.Domain.UnitOfWork;
using A1.SAS.Infrastructure.Wrappers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace A1.SAS.Api.Services.Implement
{
    public class RangeService : IRangeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RangeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<bool>> AddRangeAsync(List<RangeDto> rangeList)
        {
            var ranges = _mapper.Map<List<TblRange>>(rangeList);

            ranges.ForEach(r => r.Id = Guid.NewGuid());
            _unitOfWork.GetRepository<TblRange>().AddRange(ranges);

            await _unitOfWork.CommitAsync();

            return await Result<bool>.SuccessAsync(true);
        }

        public async Task<Result<bool>> DeleteRangeAsync(List<Guid> ids)
        {
            var ranges = await _unitOfWork.GetRepository<TblRange>()
                .GetAll()
                .Where(x => !x.IsDeleted && ids.Contains(x.Id))
                .ToListAsync();

            _unitOfWork.GetRepository<TblRange>().DeleteRange(ranges);
            await _unitOfWork.CommitAsync();

            return await Result<bool>.SuccessAsync(true) ;
        }

        public async Task<Result<IEnumerable<RangeDto>>> GetRangesAsync()
        {
            var ranges = await _unitOfWork.GetRepository<TblRange>()
                .GetAll().Where(x => !x.IsDeleted)
                .Select(x => new RangeDto { Id = x.Id, Title = x.Title, Point = x.Point })
                .OrderBy(x => x.Title)
                .ToListAsync();

            return await Result<IEnumerable<RangeDto>>.SuccessAsync(ranges);
        }

        public async Task<Result<bool>> UpdateRangeAsync(List<RangeDto> rangeList)
        {
            var ids = rangeList.Select(x => x.Id).ToHashSet();
            var ranges = await _unitOfWork.GetRepository<TblRange>()
                .GetAll()
                .Where(x => !x.IsDeleted && ids.Contains(x.Id))
                .ToListAsync();

            _mapper.Map(rangeList, ranges);

            _unitOfWork.GetRepository<TblRange>().UpdateRange(ranges);
            await _unitOfWork.CommitAsync();

            return await Result<bool>.SuccessAsync(true);
        }
    }
}
