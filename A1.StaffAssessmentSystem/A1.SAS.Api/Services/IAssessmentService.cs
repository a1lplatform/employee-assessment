using A1.SAS.Api.Dtos;
using A1.SAS.Infrastructure.Wrappers;

namespace A1.SAS.Api.Services
{
    public interface IAssessmentService
    {
        Task<Result<bool>> AddAssessmentAsync(List<AssessmentDto> assessmentList);

        Task<Result<bool>> UpdateAssessmentAsync(List<AssessmentDto> assessmentList);

        Task<Result<IEnumerable<AssessmentDto>>> GetAssessmentByEmployeeAsync(Guid employeeId);

        Task<Result<bool>> DeleteAssessmentAsync(List<Guid> ids);
    }
}
