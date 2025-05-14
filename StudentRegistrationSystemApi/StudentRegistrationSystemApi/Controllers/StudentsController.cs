using Microsoft.AspNetCore.Mvc;
using StudentRegistrationSystemApi.Data.Repositories.Interfaces;
using StudentRegistrationSystemApi.Model.DTO;
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
        public async Task<IActionResult> GetAllStudents([FromQuery] PaginationParameters pagination)
        {
            var (students, totalCount) = await _studentsRepositories.GetPaginatedStudentsAsync(pagination);

            var response = new PaginatedResponse<StudentInformationDTO>
            {
                Items = students,
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };

            return Ok(response);
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddStudent([FromForm] StudentInformationDTO student)
        {
            if (student == null)
            {
                return BadRequest("Invalid student data.");
            }

            string? filePath = null;

            if (student.FileInfo != null && student.FileInfo.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(student.FileInfo.FileName);
                filePath = Path.Combine("uploads", uniqueFileName);
                var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await student.FileInfo.CopyToAsync(stream);
                }
            }

            var studentEntity = new StudentInformation
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                MobileNumber = student.MobileNumber,
                Email = student.Email,
                NicNumber = student.NicNumber,
                DateOfBirth = student.DateOfBirth,
                ProfilePhoto = filePath,
                Address = student.Address
            };

            await _studentsRepositories.AddStudentAsync(studentEntity);

            return CreatedAtAction(nameof(GetStudentById), new { id = studentEntity.Id }, studentEntity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromForm] StudentInformationDTO student)
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

            string? filePath = existingStudent.ProfilePhoto;

            // Handle profile photo update
            if (student.FileInfo != null && student.FileInfo.Length > 0)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(existingStudent.ProfilePhoto))
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var oldFilePath = Path.Combine(uploadsFolder, existingStudent.ProfilePhoto);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Save new image
                var uploadsFolderNew = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolderNew))
                {
                    Directory.CreateDirectory(uploadsFolderNew);
                }
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(student.FileInfo.FileName);
                filePath = Path.Combine("uploads", uniqueFileName);
                var fullPath = Path.Combine(uploadsFolderNew, uniqueFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await student.FileInfo.CopyToAsync(stream);
                }
            }

            var manageDetails = new StudentInformation
            {
                Id = id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                MobileNumber = student.MobileNumber,
                Email = student.Email,
                NicNumber = student.NicNumber,
                DateOfBirth = student.DateOfBirth,
                ProfilePhoto = filePath,
                CreateBy = DateTime.Now,
                Address = student.Address
            };

            await _studentsRepositories.UpdateStudentAsync(manageDetails);
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

        [HttpGet("profile-photo/{id}")]
        public async Task<IActionResult> GetStudentProfilePhoto(int id)
        {
            var student = await _studentsRepositories.GetStudentByIdAsync(id);
            if (student == null || string.IsNullOrEmpty(student.ProfilePhoto))
            {
                return NotFound();
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var filePath = Path.Combine(uploadsFolder, student.ProfilePhoto);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var contentType = "application/octet-stream";
            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            if (ext == ".jpg" || ext == ".jpeg")
            {
                contentType = "image/jpeg";
            }
            else if (ext == ".png")
            {
                contentType = "image/png";
            }
            else if (ext == ".gif")
            {
                contentType = "image/gif";
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, contentType);
        }
    }
}