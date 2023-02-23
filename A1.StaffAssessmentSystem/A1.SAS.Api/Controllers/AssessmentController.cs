using A1.SAS.Api.Dtos;
using A1.SAS.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace A1.SAS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentController : ControllerBase
    {
        private readonly IAssessmentService _assessmentService;

        public AssessmentController(IAssessmentService assessmentService)
        {
            _assessmentService = assessmentService;
        }

        /// <summary>
        /// Get Assessment By Employee Async
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("employeeId")]
        public async Task<ActionResult<IEnumerable<AssessmentDto>>> GetAssessment(Guid employeeId)
        {

            var assessments = await _assessmentService.GetAssessmentByEmployeeAsync(employeeId);

            return Ok(assessments);
        }

        /// <summary>
        /// Create Assessment
        /// </summary>
        /// <param name="assessmentsList"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(List<AssessmentDto> assessmentsList)
        {
            var assessment = await _assessmentService.AddAssessmentAsync(assessmentsList);

            return Ok(assessment);
        }

        /// <summary>
        /// Update Assessment
        /// </summary>
        /// <param name="assessmentsList"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Update(List<AssessmentDto> assessmentsList)
        {
            await _assessmentService.UpdateAssessmentAsync(assessmentsList);

            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Delete Assessment
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteAssessment(List<Guid> ids)
        {
            await _assessmentService.DeleteAssessmentAsync(ids);

            return NoContent();
        }
    }
}
