using A1.SAS.Api.Dtos;
using A1.SAS.Infrastructure.Wrappers;

namespace A1.SAS.Api.Services
{
    public interface IAccountService
    {
        Task<Result<string>> AddAccountAsync(AccountDto accountDto, IReadOnlyList<IFormFile>? formFiles);
        Task<Result<bool>> UpdateAccountAsync(AccountDto accountDto, IReadOnlyList<IFormFile>? formFiles);
        Task<Result<IEnumerable<AccountDto>>> GetAccountsAsync();
        Task<Result<bool>> DeleteAccountAsync(Guid accountId);
        Task<Result<AccountDto>> LoginAsync(LoginDto loginDto);
        Task<Result<bool>> SetRange(Guid accountId, Guid rangeId);
        Task<Result<bool>> SetPoint(Guid accountId, int point);
        Task<Result<List<EmployeeDto>>> SearchEmployeeAsync(Guid accountId, string keyString);
    }
}
