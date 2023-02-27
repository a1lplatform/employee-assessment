using A1.SAS.Api.Dtos;
using A1.SAS.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace A1.SAS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Get all Accounts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts()
        {

            var accounts = await _accountService.GetAccountsAsync();

            return Ok(accounts);
        }

        /// <summary>
        /// Get Account By Account Code
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        [HttpGet("accountCode")]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccountByAccountCodeAsync(string accountCode)
        {

            var accountDto = await _accountService.GetAccountByAccountCodeAsync(accountCode);

            return Ok(accountDto);
        }

        /// <summary>
        /// Create Account
        /// </summary>
        /// <param name="accountDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(AccountDto accountDto)
        {
            var account = await _accountService.AddAccountAsync(accountDto);

            return Ok(account);
        }

        /// <summary>
        /// Update Account
        /// </summary>
        /// <param name="accountDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Update(AccountDto accountDto)
        {
            await _accountService.UpdateAccountAsync(accountDto);

            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _accountService.LoginAsync(loginDto);

            return Ok(result);
        }

        /// <summary>
        /// Delete Account
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{accountCode}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteAccount(string accountCode)
        {
            await _accountService.DeleteAccountAsync(accountCode);

            return NoContent();
        }
    }
}
