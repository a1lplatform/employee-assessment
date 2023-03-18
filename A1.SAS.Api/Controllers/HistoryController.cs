using A1.SAS.Api.Dtos;
using A1.SAS.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace A1.SAS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;

        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        /// <summary>
        /// Get all Range
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoryDto>>> GetHistory()
        {

            var ranges = await _historyService.GetHistoryAsync();

            return Ok(ranges);
        }
        /// <summary>
        /// Get all Range
        /// </summary>
        /// <returns></returns>
        [HttpGet("{accountId}")]
        public async Task<ActionResult<IEnumerable<HistoryDto>>> GetHistoryByAccountAsync(Guid accountId)
        {

            var ranges = await _historyService.GetHistoryByAccountAsync(accountId);

            return Ok(ranges);
        }
    }
}
