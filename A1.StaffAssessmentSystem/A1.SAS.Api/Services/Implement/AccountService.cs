
using A1.SAS.Api.Dtos;
using A1.SAS.Api.Errors;
using A1.SAS.Api.Helpers;
using A1.SAS.Domain.Entities;
using A1.SAS.Domain.UnitOfWork;
using A1.SAS.Infrastructure.Common;
using A1.SAS.Infrastructure.Wrappers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Result<string>> AddAccountAsync(AccountDto accountDto)
        {
            var account = _mapper.Map<TblAccount>(accountDto);
            account.Id = Guid.NewGuid();
            account.PasswordHash = Utils.HashPassword(accountDto.Password);
            account.AccountCode = Utils.GenerateA1PCode("NV");

            _unitOfWork.GetRepository<TblAccount>().Add(account);

            await _unitOfWork.CommitAsync();

            return await Result<string>.SuccessAsync(account.AccountCode);
        }

        public async Task<Result<bool>> DeleteAccountAsync(string accountCode)
        {
            var account = await _unitOfWork.GetRepository<TblAccount>()
                .GetAll().Where(x => x.AccountCode == accountCode && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (account == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);

            _unitOfWork.GetRepository<TblAccount>().Delete(account);

            await _unitOfWork.CommitAsync();

            return await Result<bool>.SuccessAsync(true);
        }

        public async Task<Result<AccountDto>> GetAccountByAccountCodeAsync(string accountCode)
        {
            var account =await _unitOfWork.GetRepository<TblAccount>()
                .GetAll().Where(x => x.AccountCode == accountCode && !x.IsDeleted)
                .Select(x => new AccountDto
                {
                    AccountCode = x.AccountCode,
                    FullName= x.FullName,
                    Username= x.Username,
                    Id = x.Id,
                })
                .FirstOrDefaultAsync();
            if (account == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);

            return await Result<AccountDto>.SuccessAsync(account);
        }

        public async Task<Result<IEnumerable<AccountDto>>> GetAccountsAsync()
        {
            var accounts = await _unitOfWork.GetRepository<TblAccount>()
                .GetAll().Where(x => !x.IsDeleted)
                .Select(x => new AccountDto
                {
                    AccountCode = x.AccountCode,
                    FullName = x.FullName,
                    Username = x.Username,
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

            var accountDto = new AccountDto {
                Username = account.Username,
                AccountCode= account.AccountCode,
                FullName = account.FullName,
                Id = account.Id,
            };
            return await Result<AccountDto>.SuccessAsync(accountDto);

        }

        public async Task<Result<bool>> UpdateAccountAsync(AccountDto accountDto)
        {
            var account = await _unitOfWork.GetRepository<TblAccount>().GetAsync(accountDto.Id);

            if (account == null) throw new ApiException(MessageCommon.ErrorMessage.NotFound);

            account.FullName= accountDto.FullName;
            account.Username= accountDto.Username;
            account.PasswordHash = Utils.HashPassword(accountDto.Password);

            _unitOfWork.GetRepository<TblAccount>().Update(account);

            await _unitOfWork.CommitAsync();

            return await Result<bool>.SuccessAsync(true);
        }
    }
}
