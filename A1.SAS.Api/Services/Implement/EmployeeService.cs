
using A1.SAS.Api.Dtos;
using A1.SAS.Api.Errors;
using A1.SAS.Api.Helpers;
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

        public async Task<Result<bool>> AddEmployeeAsync(PostEmployeeDto employeeDto, IReadOnlyList<IFormFile>? formFiles)
        {
            try
            {
                var employee = _mapper.Map<TblEmployee>(employeeDto);
                employee.Id = Guid.NewGuid();
                _unitOfWork.GetRepository<TblEmployee>().Add(employee);

                #region Update images
                if(formFiles != null && formFiles.Count > 0)
                {
                    var images = new List<TblImages>();
                    foreach (var file in formFiles)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString();

                            var uploadFile = new UploadHelper();

                            uploadFile.UploadFile(file, fileName);

                            images.Add(new TblImages { 
                                URL = $"{fileName}.jpg", 
                                EmployeeId = employee.Id, 
                                Id = Guid.NewGuid(), 
                                AccountId = null 
                            });
                        }
                    }
                    if(images.Count > 0) _unitOfWork.GetRepository<TblImages>().AddRange(images);
                }
                #endregion

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

        public async Task<Result<IEnumerable<EmployeeDto>>> GetEmployeesAsync()
        {
            var employees = await (_unitOfWork.GetRepository<TblEmployee>()
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
                    Assessments = _unitOfWork.GetRepository<TblAssessment>()
                                            .GetAll()
                                            .Where(x => !x.IsDeleted && e.Id == x.EmployeeId)
                                            .Select(x => new AssessmentDto
                                            {
                                                AssessmentDate = x.AssessmentDate,
                                                Content = x.Content,
                                                EmployeeId = x.EmployeeId,
                                                IsActive = x.IsActive
                                            })
                                            .ToList(),
                    Images = _unitOfWork.GetRepository<TblImages>()
                                                    .GetAll()
                                                    .Where(a => a.EmployeeId == e.Id && !a.IsDeleted)
                                                    .Select(a => new ImageDtos
                                                    {
                                                        Id = a.Id,
                                                        EmployeeId = a.EmployeeId,
                                                        URL = a.URL,
                                                    }).ToList()
                }))
                .ToListAsync();            

            return await Result<IEnumerable<EmployeeDto>>.SuccessAsync(employees);
        }

        public async Task<Result<bool>> UpdateEmployeeAsync(PostEmployeeDto employeeDto, IReadOnlyList<IFormFile>? formFiles)
        {
            var employee = await _unitOfWork.GetRepository<TblEmployee>()
                .GetAll()
                .Where(x => x.Id == employeeDto.Id && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (employee == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);

            _mapper.Map(employeeDto, employee);

            _unitOfWork.GetRepository<TblEmployee>().Update(employee);

            #region Update images
            if (formFiles != null && formFiles.Count > 0)
            {
                var images = new List<TblImages>();
                // remove all image old
                if(employeeDto.Images != null && employeeDto.Images.Count > 0) {
                    foreach (var image in employeeDto.Images)
                    {
                        new UploadHelper().DeleteFile(image.URL);
                        images.Add(new TblImages
                        {
                            URL = image.URL,
                            EmployeeId = image.EmployeeId,
                            Id = image.Id,
                            AccountId = image.AccountId,
                            IsDeleted = true
                        });
                    }
                }        
                
                foreach (var file in formFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString();

                        var uploadFile = new UploadHelper();

                        uploadFile.UploadFile(file, fileName);

                        images.Add(new TblImages
                        {
                            URL = $"{fileName}.jpg",
                            EmployeeId = employee.Id,
                            Id = Guid.NewGuid(),
                            AccountId = null
                        });
                    }
                }
                if (images.Count > 0) _unitOfWork.GetRepository<TblImages>().AddRange(images);
            }
            #endregion

            await _unitOfWork.CommitAsync();

            return await Result<bool>.SuccessAsync(true);
        }

        public async Task<Result<EmployeeDto>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await (_unitOfWork.GetRepository<TblEmployee>()
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
                    Id = e.Id,
                    Assessments = _unitOfWork.GetRepository<TblAssessment>()
                                            .GetAll()
                                            .Where(x => !x.IsDeleted && e.Id == x.EmployeeId)
                                            .Select(x => new AssessmentDto
                                            {
                                                AssessmentDate = x.AssessmentDate,
                                                Content = x.Content,
                                                EmployeeId = x.EmployeeId,
                                                IsActive = x.IsActive
                                            })
                                            .ToList(),
                    Images = _unitOfWork.GetRepository<TblImages>()
                                                    .GetAll()
                                                    .Where(a => a.EmployeeId == e.Id && !a.IsDeleted)
                                                    .Select(a => new ImageDtos
                                                    {
                                                        Id = a.Id,
                                                        EmployeeId = a.EmployeeId,
                                                        URL = a.URL,
                                                    }).ToList()
                }))
                .FirstOrDefaultAsync();            

            return await Result<EmployeeDto>.SuccessAsync(employee);
        }
    }
}
