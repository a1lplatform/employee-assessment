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
        /// Register Account
        /// </summary>
        /// <param name="accountDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<Guid>> Register(AccountDto accountDto)
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
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteAccount(Guid id)
        {
            await _accountService.DeleteAccountAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Search Employee Async
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> SearchEmployeeAsync(Guid accountId, string keyString)
        {
            var result = await _accountService.SearchEmployeeAsync(accountId, keyString);

            return Ok(result);
        }
    }
}
