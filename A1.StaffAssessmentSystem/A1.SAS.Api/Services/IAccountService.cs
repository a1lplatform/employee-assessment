using A1.SAS.Api.Dtos;
using A1.SAS.Infrastructure.Wrappers;

namespace A1.SAS.Api.Services
{
    public interface IAccountService
    {
        Task<Result<string>> AddAccountAsync(AccountDto accountDto);

        Task<Result<bool>> UpdateAccountAsync(AccountDto accountDto);

        Task<Result<IEnumerable<AccountDto>>> GetAccountsAsync();

        Task<Result<AccountDto>> GetAccountByAccountCodeAsync(string accountCode);

        Task<Result<bool>> DeleteAccountAsync(string accountCode);

        Task<Result<AccountDto>> LoginAsync(LoginDto loginDto);
    }
}
