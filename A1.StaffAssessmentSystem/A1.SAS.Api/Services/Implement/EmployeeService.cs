
using A1.SAS.Api.Dtos;
using A1.SAS.Api.Errors;
using A1.SAS.Domain.Entities;
using A1.SAS.Domain.UnitOfWork;
using A1.SAS.Infrastructure.Common;
using A1.SAS.Infrastructure.Wrappers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace A1.SAS.Api.Services.Implement
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<bool>> AddEmployeeAsync(PostEmployeeDto employeeDto)
        {
            try
            {
                var employee = _mapper.Map<TblEmployee>(employeeDto);
                employee.Id = Guid.NewGuid();
                employee.Range.Id = Guid.NewGuid();
                _unitOfWork.GetRepository<TblEmployee>().Add(employee);

                await _unitOfWork.CommitAsync();
                return await Result<bool>.SuccessAsync(true);
            }
            catch (Exception e)
            {
                throw new ApiException(e.Message);
            }            
        }

        public async Task<Result<bool>> DeleteEmployeeAsync(Guid id)
        {
            var employee = await _unitOfWork.GetRepository<TblEmployee>().GetAsync(id);

            if (employee == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);

            _unitOfWork.GetRepository<TblEmployee>().Delete(employee);

            await _unitOfWork.CommitAsync();

            return await Result<bool>.SuccessAsync(true);
        }

        public async Task<Result<EmployeeDto>> GetEmployeeByPartPostIdAsync(string partPostId)
        {
            if (partPostId == null) throw new ArgumentNullException(nameof(partPostId));

            var employee = await _unitOfWork.GetRepository<TblEmployee>()
                .GetAll()
                .Include(e => e.Range)
                .Where(e => !e.IsDeleted && e.PartpostId.Equals(partPostId))
                .Select(e => new EmployeeDto
                {
                    PartpostId = e.PartpostId,
                    BirthDate = e.BirthDate,
                    FullName = e.FullName,
                    Gender = e.Gender,
                    Id = e.Id,
                    Range = new RangeDto { Title = e.Range.Title , Id = e.Range.Id}
                })
                .FirstOrDefaultAsync();

            if (employee == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);

            employee.Assessments = await _unitOfWork.GetRepository<TblAssessment>()
                .GetAll()
                .Include(x => x.Employee)
                .Where(x => !x.IsDeleted && x.EmployeeId == employee.Id)
                .Select(x => new AssessmentDto
                {
                    AssessmentDate = x.AssessmentDate,
                    Content = x.Content,
                    EmployeeId = x.Id,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return await Result<EmployeeDto>.SuccessAsync(employee);
        }

        public async Task<Result<IEnumerable<EmployeeDto>>> GetEmployeesAsync()
        {
            var employees = await _unitOfWork.GetRepository<TblEmployee>()
                .GetAll()
                .Include(e => e.Range)
                .Where(e => !e.IsDeleted)
                .Select(e => new EmployeeDto
                {
                    PartpostId = e.PartpostId,
                    BirthDate = e.BirthDate,
                    FullName = e.FullName,
                    Gender = e.Gender,
                    Id = e.Id,
                    Range = new RangeDto { Title = e.Range.Title, Id = e.Range.Id}
                })
                .ToListAsync();

            if (employees == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);
            var employeeIds = employees.Select(x => x.Id).ToHashSet();
            var assessments =  await _unitOfWork.GetRepository<TblAssessment>()
                .GetAll()
                .Where(x => !x.IsDeleted && employeeIds.Contains(x.EmployeeId))
                .Select(x => new AssessmentDto
                {
                    AssessmentDate = x.AssessmentDate,
                    Content = x.Content,
                    EmployeeId = x.EmployeeId,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            foreach (var emp in employees)
            {
                emp.Assessments = assessments?.Where(x => x.EmployeeId.Equals(emp.Id)).ToList();
            }

            return await Result<IEnumerable<EmployeeDto>>.SuccessAsync(employees);
        }

        public async Task<Result<bool>> UpdateEmployeeAsync(PostEmployeeDto employeeDto)
        {
            var employee = await _unitOfWork.GetRepository<TblEmployee>()
                .GetAll().Include(x => x.Range)
                .Where(x => x.Id == employeeDto.Id && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (employee == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);

            _mapper.Map(employeeDto, employee);

            _unitOfWork.GetRepository<TblEmployee>().Update(employee);

            await _unitOfWork.CommitAsync();

            return await Result<bool>.SuccessAsync(true);
        }
    }
}
