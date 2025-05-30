using Microsoft.AspNetCore.Mvc;
using University2.REST.Interfaces;
using University2.REST.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace University2.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly ICrudServiceAsync<StudentModel> _studentService;

        public StudentsController(ICrudServiceAsync<StudentModel> studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentModel>>> GetStudents()
        {
            var students = await _studentService.ReadAllAsync();
            return Ok(students);
        }

        [HttpGet("paged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<StudentModel>>> GetStudentsPaged(
            [FromQuery] int page = 1,
            [FromQuery] int amount = 10)
        {
            if (page < 1 || amount < 1)
                return BadRequest("Page and amount must be positive integers.");

            var students = await _studentService.ReadAllAsync(page, amount);
            return Ok(students);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentModel>> GetStudent(Guid id)
        {
            var student = await _studentService.ReadAsync(id);
            if (student == null)
                return NotFound();

            return Ok(student);
        }

        [HttpPost]
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
                return BadRequest("Could not create student.");

            await _studentService.SaveAsync();

            return CreatedAtAction(nameof(GetStudent),
                new { id = studentModel.Id },
                studentModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutStudent(
            Guid id,
            [FromBody] StudentUpdateModel studentUpdateDto)
        {
            if (id != studentUpdateDto.Id)
                return BadRequest("ID in URL does not match ID in body.");

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
                return BadRequest("Could not update student.");

            await _studentService.SaveAsync();

            return Ok(existingStudent);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _studentService.ReadAsync(id);
            if (student == null)
                return NotFound();

            var success = await _studentService.RemoveAsync(student);
            if (!success)
                return BadRequest("Could not delete student.");

            await _studentService.SaveAsync();

            return NoContent();
        }
    }
}