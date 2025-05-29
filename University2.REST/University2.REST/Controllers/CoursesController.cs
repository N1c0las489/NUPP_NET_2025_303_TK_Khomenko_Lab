using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University2.REST.Models;
using University2.REST.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace University2.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICrudServiceAsync<CourseModel> _courseService;

        public CoursesController(ICrudServiceAsync<CourseModel> courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        [AllowAnonymous] // Дозволити доступ без авторизації
        public async Task<ActionResult<IEnumerable<CourseModel>>> GetCourses()
        {
            var courses = await _courseService.ReadAllAsync();
            return Ok(courses);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous] // Дозволити доступ без авторизації
        public async Task<ActionResult<CourseModel>> GetCourse(int id)
        {
            var course = await _courseService.ReadAsync(id);
            if (course == null)
                return NotFound($"Course with ID {id} not found");

            return Ok(course);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")] // Тільки адміни та викладачі
        public async Task<ActionResult<CourseModel>> PostCourse(
            [FromBody] CourseCreateModel dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var course = new CourseModel
            {
                Title = dto.Title,
                Credits = dto.Credits
            };

            var created = await _courseService.CreateAsync(course);
            if (!created)
                return BadRequest("Failed to create course");

            await _courseService.SaveAsync();

            return CreatedAtAction(
                nameof(GetCourse),
                new { id = course.Id },
                course);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")] // Тільки адміни та викладачі
        public async Task<IActionResult> UpdateCourse(
            int id,
            [FromBody] CourseUpdateModel dto)
        {
            if (id != dto.Id)
                return BadRequest("ID in URL does not match ID in body");

            var existingCourse = await _courseService.ReadAsync(id);
            if (existingCourse == null)
                return NotFound();

            existingCourse.Title = dto.Title;
            existingCourse.Credits = dto.Credits;

            var updated = await _courseService.UpdateAsync(existingCourse);
            if (!updated)
                return BadRequest("Failed to update course");

            await _courseService.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")] // Тільки адміни
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _courseService.ReadAsync(id);
            if (course == null)
                return NotFound();

            var deleted = await _courseService.RemoveAsync(course);
            if (!deleted)
                return BadRequest("Failed to delete course");

            await _courseService.SaveAsync();

            return NoContent();
        }
    }

    public class CourseUpdateModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
    }
}