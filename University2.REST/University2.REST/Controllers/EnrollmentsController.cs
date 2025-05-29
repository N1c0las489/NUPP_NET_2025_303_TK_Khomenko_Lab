using Microsoft.AspNetCore.Mvc;
using University2.REST.Models;
using University2.REST.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly ICrudServiceAsync<EnrollmentModel> _enrollmentService;

    public EnrollmentsController(ICrudServiceAsync<EnrollmentModel> enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpPost]
    public async Task<ActionResult<EnrollmentModel>> PostEnrollment(
        [FromBody] EnrollmentCreateModel dto)
    {
        var enrollment = new EnrollmentModel
        {
            StudentId = dto.StudentId,
            CourseId = dto.CourseId,
            Grade = dto.Grade
        };

        await _enrollmentService.CreateAsync(enrollment);
        return CreatedAtAction(nameof(GetEnrollment), new { id = enrollment.Id }, enrollment);
    }

    private object GetEnrollment()
    {
        throw new NotImplementedException();
    }
}