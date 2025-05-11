using Microsoft.AspNetCore.Mvc;
using StudentRegistrationSystemApi.Data.Repositories.Interfaces;
using StudentRegistrationSystemApi.Model.Entity;

namespace StudentRegistrationSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentsRepositories _studentsRepositories;
        public StudentsController(IStudentsRepositories studentsRepositories)
        {
            _studentsRepositories = studentsRepositories;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentsRepositories.GetAllStudentsAsync();
            return Ok(students);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentsRepositories.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }
        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] StudentInformation student)
        {
            if (student == null)
            {
                return BadRequest();
            }
            await _studentsRepositories.AddStudentAsync(student);
            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentInformation student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }
            var existingStudent = await _studentsRepositories.GetStudentByIdAsync(id);
            if (existingStudent == null)
            {
                return NotFound();
            }
            await _studentsRepositories.UpdateStudentAsync(student);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _studentsRepositories.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            await _studentsRepositories.DeleteStudentAsync(id);
            return NoContent();
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchStudents([FromQuery] string searchTerm)
        {
            var students = await _studentsRepositories.GetAllStudentsAsync();
            var filteredStudents = students.Where(s => s.FirstName.Contains(searchTerm) || s.LastName.Contains(searchTerm));
            return Ok(filteredStudents);
        }
    }
}