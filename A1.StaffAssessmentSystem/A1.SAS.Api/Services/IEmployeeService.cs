﻿using A1.SAS.Api.Dtos;
using A1.SAS.Infrastructure.Wrappers;

namespace A1.SAS.Api.Services
{
    public interface IEmployeeService
    {
        Task<Result<bool>> AddEmployeeAsync(PostEmployeeDto employeeDto);

        Task<Result<bool>> UpdateEmployeeAsync(PostEmployeeDto employeeDto);

        Task<Result<IEnumerable<EmployeeDto>>> GetEmployeesAsync();

        Task<Result<EmployeeDto>> GetEmployeeByPartPostIdAsync(string partPostId);

        Task<Result<bool>> DeleteEmployeeAsync(Guid id);
    }
}