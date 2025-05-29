using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University2.REST.Models;
using University2.REST.Interfaces;
using System;
using System.Threading.Tasks;

namespace University2.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class EnrollmentsController : ControllerBase
    {
        private readonly ICrudServiceAsync<EnrollmentModel> _enrollmentService;

        public EnrollmentsController(ICrudServiceAsync<EnrollmentModel> enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous] 
        public async Task<ActionResult<EnrollmentModel>> GetEnrollment(int id)
        {
            var enrollment = await _enrollmentService.ReadAsync(id);
            if (enrollment == null)
                return NotFound($"Enrollment with ID {id} not found");

            return Ok(enrollment);
        }

        [HttpPost]
        [Authorize(Roles = "Адмін,Викладач,Студент")] 
        public async Task<ActionResult<EnrollmentModel>> PostEnrollment(
            [FromBody] EnrollmentCreateModel dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            if (User.IsInRole("Student"))
            {
                var studentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (studentId != dto.StudentId.ToString())
                {
                    return Forbid(); 
                }
            }

            var enrollment = new EnrollmentModel
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                Grade = dto.Grade
            };

            var created = await _enrollmentService.CreateAsync(enrollment);
            if (!created)
                return BadRequest("Failed to create enrollment");

            await _enrollmentService.SaveAsync();

            return CreatedAtAction(
                nameof(GetEnrollment),
                new { id = enrollment.Id },
                enrollment);
        }

        [HttpPut("{id:int}/grade")]
        [Authorize(Roles = "Адмін,Викладач")] 
        public async Task<IActionResult> UpdateGrade(
            int id,
            [FromBody] GradeUpdateModel dto)
        {
            var enrollment = await _enrollmentService.ReadAsync(id);
            if (enrollment == null)
                return NotFound();

            enrollment.Grade = dto.Grade;

            var updated = await _enrollmentService.UpdateAsync(enrollment);
            if (!updated)
                return BadRequest("Не вдалося оновити оцінку");

            await _enrollmentService.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Адмін")] 
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enrollment = await _enrollmentService.ReadAsync(id);
            if (enrollment == null)
                return NotFound();

            var deleted = await _enrollmentService.RemoveAsync(enrollment);
            if (!deleted)
                return BadRequest("Не вдалося видалити реєстрацію");

            await _enrollmentService.SaveAsync();

            return NoContent();
        }
    }

    public class GradeUpdateModel
    {
        public Grade? Grade { get; set; }
    }
}