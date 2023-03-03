using A1.SAS.Api.Dtos;
using A1.SAS.Infrastructure.Wrappers;

namespace A1.SAS.Api.Services
{
    public interface IEmployeeService
    {
        Task<Result<bool>> AddEmployeeAsync(PostEmployeeDto employeeDto, IReadOnlyList<IFormFile>? formFiles);

        Task<Result<bool>> UpdateEmployeeAsync(PostEmployeeDto employeeDto, IReadOnlyList<IFormFile>? formFiles);

        Task<Result<IEnumerable<EmployeeDto>>> GetEmployeesAsync();

        Task<Result<bool>> DeleteEmployeeAsync(Guid id);

        Task<Result<EmployeeDto>> GetEmployeeByIdAsync(Guid id);
    }
}
