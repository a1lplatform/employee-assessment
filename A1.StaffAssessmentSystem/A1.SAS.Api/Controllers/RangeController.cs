using A1.SAS.Api.Dtos;
using A1.SAS.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace A1.SAS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RangeController : ControllerBase
    {
        private readonly IRangeService _rangeService;

        public RangeController(IRangeService rangeService)
        {
            _rangeService = rangeService;
        }

        /// <summary>
        /// Get all Range
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RangeDto>>> GetRange()
        {

            var ranges = await _rangeService.GetRangesAsync();

            return Ok(ranges);
        }

        /// <summary>
        /// Create Range
        /// </summary>
        /// <param name="rangesList"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(List<RangeDto> rangesList)
        {
            var range = await _rangeService.AddRangeAsync(rangesList);

            return Ok(range);
        }

        /// <summary>
        /// Update Range
        /// </summary>
        /// <param name="rangesList"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Update(List<RangeDto> rangesList)
        {
            await _rangeService.UpdateRangeAsync(rangesList);

            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Delete Range
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteRange(List<Guid> ids)
        {
            await _rangeService.DeleteRangeAsync(ids);

            return NoContent();
        }
    }
}
