
using A1.SAS.Api.Dtos;
using A1.SAS.Domain.Entities;
using A1.SAS.Domain.UnitOfWork;
using A1.SAS.Infrastructure.Wrappers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace A1.SAS.Api.Services.Implement
{
    public class AssessmentService : IAssessmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AssessmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<bool>> AddAssessmentAsync(List<AssessmentDto> assessmentDtos)
        {
            var assessments = _mapper.Map<List<TblAssessment>>(assessmentDtos);

            _unitOfWork.GetRepository<TblAssessment>().AddRange(assessments);

            await _unitOfWork.CommitAsync();

            return await Result<bool>.SuccessAsync(true);
        }

        public async Task<Result<bool>> DeleteAssessmentAsync(List<Guid> ids)
        {
            var assessments = await _unitOfWork.GetRepository<TblAssessment>()
                .GetAll()
                .Where(x => !x.IsDeleted && ids.Contains(x.Id))
                .ToListAsync();

            _unitOfWork.GetRepository<TblAssessment>().DeleteRange(assessments);
            await _unitOfWork.CommitAsync();

            return await Result<bool>.SuccessAsync(true) ;
        }

        public async Task<Result<IEnumerable<AssessmentDto>>> GetAssessmentByEmployeeAsync(Guid employeeId)
        {
            var assessments = await _unitOfWork.GetRepository<TblAssessment>()
                .GetAll().Where(x => !x.IsDeleted && x.EmployeeId.Equals(employeeId))
                .Select(x => new AssessmentDto
                {
                    AssessmentDate = x.AssessmentDate,
                    Content = x.Content,
                    EmployeeId = x.EmployeeId,
                    IsActive = x.IsActive,
                    Id = x.Id,
                })
                .ToListAsync();

            return await Result<IEnumerable<AssessmentDto>>.SuccessAsync(assessments);
        }

        public async Task<Result<bool>> UpdateAssessmentAsync(List<AssessmentDto> assessmentDtos)
        {
            var ids = assessmentDtos.Select(x => x.Id).ToHashSet();
            var assessments = await _unitOfWork.GetRepository<TblAssessment>()
                .GetAll()
                .Where(x => !x.IsDeleted && ids.Contains(x.Id))
                .ToListAsync();

            _mapper.Map(assessmentDtos, assessments);

            _unitOfWork.GetRepository<TblAssessment>().UpdateRange(assessments);
            await _unitOfWork.CommitAsync();

            return await Result<bool>.SuccessAsync(true);
        }
    }
}
