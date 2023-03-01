
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
                _unitOfWork.GetRepository<TblEmployee>().Add(employee);

                if(employeeDto.Images != null && employeeDto.Images.Any())
                {
                    var images = employeeDto.Images
                        .Select(x => new TblImages { 
                            AccountId = null,
                            URL = x.URL,
                            EmployeeId = employee.Id,
                        }).ToList();
                    _unitOfWork.GetRepository<TblImages>().AddRange(images);
                }               

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

        public async Task<Result<EmployeeDto>> SearchEmployeeAsync(string keyString)
        {
            if (string.IsNullOrEmpty(keyString)) throw new ArgumentNullException(nameof(keyString));

            var employee = await _unitOfWork.GetRepository<TblEmployee>()
                .GetAll()
                .Where(e => !e.IsDeleted && e.CCCD.Contains(keyString))
                .Select(e => new EmployeeDto
                {
                    CCCD= e.CCCD,
                    PhoneNo= e.PhoneNo,
                    Gender= e.Gender,
                    Address= e.Address,
                    Birthday= e.Birthday,
                    Email= e.Email,
                    FullName= e.FullName,
                    Id= e.Id                    
                })
                .FirstOrDefaultAsync();

            if (employee == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);

            return await Result<EmployeeDto>.SuccessAsync(employee);
        }

        public async Task<Result<IEnumerable<EmployeeDto>>> GetEmployeesAsync()
        {
            var employees = await _unitOfWork.GetRepository<TblEmployee>()
                .GetAll()
                .Where(e => !e.IsDeleted)
                .Select(e => new EmployeeDto
                {
                    Id= e.Id,
                    Address= e.Address,
                    Birthday= e.Birthday,
                    CCCD= e.CCCD,
                    Email= e.Email,
                    FullName= e.FullName,
                    Gender= e.Gender,
                    PhoneNo= e.PhoneNo,
                })
                .ToListAsync();

            if (employees == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);
            var employeeIds = employees.Select(x => x.Id).ToHashSet();
            var assessments = await _unitOfWork.GetRepository<TblAssessment>()
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
                .GetAll()
                .Where(x => x.Id == employeeDto.Id && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (employee == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);

            _mapper.Map(employeeDto, employee);

            _unitOfWork.GetRepository<TblEmployee>().Update(employee);

            await _unitOfWork.CommitAsync();

            return await Result<bool>.SuccessAsync(true);
        }

        public async Task<Result<EmployeeDto>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _unitOfWork.GetRepository<TblEmployee>()
                .GetAll()
                .Where(e => !e.IsDeleted && e.Id.Equals(id))
                .Select(e => new EmployeeDto
                {
                    CCCD = e.CCCD,
                    PhoneNo = e.PhoneNo,
                    Gender = e.Gender,
                    Address = e.Address,
                    Birthday = e.Birthday,
                    Email = e.Email,
                    FullName = e.FullName,
                    Id = e.Id
                })
                .FirstOrDefaultAsync();

            if (employee == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);

            employee.Assessments = await _unitOfWork.GetRepository<TblAssessment>()
                .GetAll()
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
    }
}
