using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University2.REST.Interfaces;
using University2.REST.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

namespace University2.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly ICrudServiceAsync<StudentModel> _studentService;

        public StudentsController(ICrudServiceAsync<StudentModel> studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentModel>>> GetStudents()
        {
            var students = await _studentService.ReadAllAsync();
            return Ok(students);
        }

        [HttpGet("paged")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<StudentModel>>> GetStudentsPaged(
            [FromQuery] int page = 1,
            [FromQuery] int amount = 10)
        {
            if (page < 1 || amount < 1)
                return BadRequest("Сторінка та сума мають бути додатними цілими числами.");

            var students = await _studentService.ReadAllAsync(page, amount);
            return Ok(students);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Адмін,Викладач,Студент")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentModel>> GetStudent(Guid id)
        {
            if (User.IsInRole("Студент") && !IsCurrentUser(id))
                return Forbid();

            var student = await _studentService.ReadAsync(id);
            if (student == null)
                return NotFound();

            return Ok(student);
        }

        [HttpPost]
        [Authorize(Roles = "Адмін")] 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StudentModel>> PostStudent(
            [FromBody] StudentCreateModel studentCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var studentModel = new StudentModel
            {
                FirstName = studentCreateDto.FirstName,
                LastName = studentCreateDto.LastName,
                BirthDate = studentCreateDto.BirthDate
            };

            var success = await _studentService.CreateAsync(studentModel);
            if (!success)
                return BadRequest("Не вдалося створити студента.");

            await _studentService.SaveAsync();

            return CreatedAtAction(nameof(GetStudent),
                new { id = studentModel.Id },
                studentModel);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Адмін,Студент")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutStudent(
            Guid id,
            [FromBody] StudentUpdateModel studentUpdateDto)
        {
            if (User.IsInRole("Студент") && !IsCurrentUser(id))
                return Forbid();

            if (id != studentUpdateDto.Id)
                return BadRequest("Ідентифікатор в URL-адресі не збігається з ідентифікатором у тілі.");

            var existingStudent = await _studentService.ReadAsync(id);
            if (existingStudent == null)
                return NotFound();

            if (!string.IsNullOrEmpty(studentUpdateDto.FirstName))
                existingStudent.FirstName = studentUpdateDto.FirstName;

            if (!string.IsNullOrEmpty(studentUpdateDto.LastName))
                existingStudent.LastName = studentUpdateDto.LastName;

            if (studentUpdateDto.BirthDate != default)
                existingStudent.BirthDate = studentUpdateDto.BirthDate;

            var success = await _studentService.UpdateAsync(existingStudent);
            if (!success)
                return BadRequest("Не вдалося оновити інформацію про студента.");

            await _studentService.SaveAsync();

            return Ok(existingStudent);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Адмін")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _studentService.ReadAsync(id);
            if (student == null)
                return NotFound();

            var success = await _studentService.RemoveAsync(student);
            if (!success)
                return BadRequest("Не вдалося видалити інформацію про студента.");

            await _studentService.SaveAsync();

            return NoContent();
        }

        private bool IsCurrentUser(Guid studentId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return currentUserId != null && Guid.Parse(currentUserId) == studentId;
        }
    }
}
