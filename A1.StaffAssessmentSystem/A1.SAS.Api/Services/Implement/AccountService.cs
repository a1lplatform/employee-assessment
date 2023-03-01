
using A1.SAS.Api.Dtos;
using A1.SAS.Api.Errors;
using A1.SAS.Api.Helpers;
using A1.SAS.Domain.Entities;
using A1.SAS.Domain.UnitOfWork;
using A1.SAS.Infrastructure.Common;
using A1.SAS.Infrastructure.Wrappers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Security.Authentication;

namespace A1.SAS.Api.Services.Implement
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<bool>> SetPoint(Guid accountId, int point)
        {
            var account = await _unitOfWork.GetRepository<TblAccount>()
                .GetAll().Where(x => x.Id == accountId && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (account == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);

            account.Point = point;

            _unitOfWork.GetRepository<TblAccount>().Update(account);

            await _unitOfWork.CommitAsync();

            return await Result<bool>.SuccessAsync(true);
        }

        public async Task<Result<bool>> SetRange(Guid accountId, Guid rangeId)
        {
            var account = await _unitOfWork.GetRepository<TblAccount>()
                .GetAll().Where(x => x.Id == accountId && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (account == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);

            account.RangeId = rangeId;

            _unitOfWork.GetRepository<TblAccount>().Update(account);

            await _unitOfWork.CommitAsync();

            return await Result<bool>.SuccessAsync(true);
        }

        public async Task<Result<string>> AddAccountAsync(AccountDto accountDto)
        {
            var account = _mapper.Map<TblAccount>(accountDto);
            account.Id = Guid.NewGuid();
            account.PasswordHash = Utils.HashPassword(accountDto.Password);

            _unitOfWork.GetRepository<TblAccount>().Add(account);

            await _unitOfWork.CommitAsync();

            return await Result<string>.SuccessAsync(account.Id.ToString());
        }

        public async Task<Result<bool>> DeleteAccountAsync(Guid accountId)
        {
            var account = await _unitOfWork.GetRepository<TblAccount>()
                .GetAll().Where(x => x.Id == accountId && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (account == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);

            _unitOfWork.GetRepository<TblAccount>().Delete(account);

            await _unitOfWork.CommitAsync();

            return await Result<bool>.SuccessAsync(true);
        }

        public async Task<Result<IEnumerable<AccountDto>>> GetAccountsAsync()
        {
            var accounts = await _unitOfWork.GetRepository<TblAccount>()
                .GetAll().Where(x => !x.IsDeleted)
                .Select(x => new AccountDto
                {
                    FullName = x.FullName,
                    Username = x.Username,
                    Address= x.Address,
                    Birthday=x.Birthday,
                    CCCD=x.CCCD,
                    Email=x.Email,
                    Gender=x.Gender,
                    PhoneNo=x.PhoneNo,
                    Point=x.Point,
                    RangeId=x.RangeId,
                    RoleName=x.RoleName,
                    Id = x.Id,
                })
                .ToListAsync();

            return await Result<IEnumerable<AccountDto>>.SuccessAsync(accounts);
        }

        public async Task<Result<AccountDto>> LoginAsync(LoginDto loginDto)
        {
            var account = await _unitOfWork.GetRepository<TblAccount>()
                .GetAll()
                .Where(x => !x.IsDeleted && x.Username == loginDto.Username)
                .FirstOrDefaultAsync();

            if (account == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);

            if (!Utils.VerifyPassword(loginDto.Password, account.PasswordHash))
                throw new AuthenticationException();

            var accountDto = new AccountDto
            {
                FullName = account.FullName,
                Username = account.Username,
                Address = account.Address,
                Birthday = account.Birthday,
                CCCD = account.CCCD,
                Email = account.Email,
                Gender = account.Gender,
                PhoneNo = account.PhoneNo,
                Point = account.Point,
                RangeId = account.RangeId,
                RoleName = account.RoleName,
                Id = account.Id,
            };
            return await Result<AccountDto>.SuccessAsync(accountDto);

        }

        public async Task<Result<bool>> UpdateAccountAsync(AccountDto accountDto)
        {
            var account = await _unitOfWork.GetRepository<TblAccount>().GetAsync(accountDto.Id);

            if (account == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);

            _mapper.Map(accountDto, account);
            account.PasswordHash = Utils.HashPassword(accountDto.Password);

            _unitOfWork.GetRepository<TblAccount>().Update(account);

            await _unitOfWork.CommitAsync();

            return await Result<bool>.SuccessAsync(true);
        }

        public async Task<Result<List<EmployeeDto>>> SearchEmployeeAsync(Guid accountId, string keyString)
        {
            #region Check the remaining points of the account

            var account = await _unitOfWork.GetRepository<TblAccount>().GetAsync(accountId);
            if (account == null) throw new InvalidOperationException();

            var accountRange = await _unitOfWork.GetRepository<TblRange>()
                .GetAll()
                .Where(x => !x.IsDeleted && x.Id == account.RangeId)
                .FirstOrDefaultAsync();

            if (accountRange == null) throw new InvalidOperationException();
            if (account.Point - accountRange.Point < 0) throw new ApiException(MessageCommon.ErrorMessage.NotRemainingPoint);

            #endregion

            if (string.IsNullOrEmpty(keyString)) throw new ArgumentNullException(nameof(keyString));

            var employee = await (_unitOfWork.GetRepository<TblEmployee>().GetAll()
                            .Where(x => !x.IsDeleted && x.CCCD.Contains(keyString))
                            .Select(x => new EmployeeDto
                            {
                                CCCD= x.CCCD,
                                Address= x.Address,
                                Birthday= x.Birthday,
                                Email= x.Email,
                                FullName= x.FullName,
                                Gender= x.Gender,
                                Id= x.Id,
                                PhoneNo= x.PhoneNo,
                                Assessments = _unitOfWork.GetRepository<TblAssessment>()
                                                    .GetAll()
                                                    .Where(a => a.EmployeeId == x.Id && !a.IsDeleted)
                                                    .Select(a => new AssessmentDto { 
                                                        Id= a.Id,
                                                        EmployeeId= a.EmployeeId,
                                                        AssessmentDate = a.AssessmentDate,
                                                        Content= a.Content,
                                                        IsActive = a.IsActive
                                                    }).ToList(),
                                Images = _unitOfWork.GetRepository<TblImages>()
                                                    .GetAll()
                                                    .Where(a => a.EmployeeId == x.Id && !a.IsDeleted)
                                                    .Select(a => new ImageDtos
                                                    {
                                                        Id = a.Id,
                                                        EmployeeId= a.EmployeeId,
                                                        URL= a.URL,
                                                    }).ToList(),
                            })).ToListAsync();

            #region deduct points from the account and add history

            account.Point = account.Point - accountRange.Point;

            _unitOfWork.GetRepository<TblAccount>().Update(account);

            var history = new TblHistory { 
                Id = Guid.NewGuid(),
                CreatedBy = accountId.ToString(),
                SearchContent = keyString
            };
            _unitOfWork.GetRepository<TblHistory>().Add(history);
            await _unitOfWork.CommitAsync();
            #endregion

            return await Result<List<EmployeeDto>>.SuccessAsync(employee);
        }
    }
}
