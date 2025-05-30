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
        public async Task<ActionResult<IEnumerable<CourseModel>>> GetCourses()
        {
            var courses = await _courseService.ReadAllAsync();
            return Ok(courses);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CourseModel>> GetCourse(int id)
        {
            var course = await _courseService.ReadAsync(id);
            if (course == null)
                return NotFound($"Course with ID {id} not found");

            return Ok(course);
        }

        [HttpPost]
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
    }
}