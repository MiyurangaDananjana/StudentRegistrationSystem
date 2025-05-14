using Microsoft.EntityFrameworkCore;
using Serilog;
using StudentRegistrationSystemApi.Data.Repositories.Interfaces;
using StudentRegistrationSystemApi.Model.DTO;
using StudentRegistrationSystemApi.Model.Entity;

namespace StudentRegistrationSystemApi.Data.Repositories.Implementations
{
    public class StudentsRepositories : IStudentsRepositories
    {
        private readonly AppDbContext _context;
        public StudentsRepositories(AppDbContext context)
        {
            _context = context;
        }

        public Task AddStudentAsync(StudentInformation student)
        {
            _context.StudentInformations.AddAsync(student);
            return _context.SaveChangesAsync();
        }

        public Task DeleteStudentAsync(int id)
        {
            try
            {
                var student = _context.StudentInformations.Find(id);
                if (student == null)
                {
                    Log.Warning("DeleteStudentAsync: Student with ID {StudentId} not found.", id);
                    return Task.CompletedTask;
                }

                _context.StudentInformations.Remove(student);
                Log.Information("DeleteStudentAsync: Student with ID {StudentId} successfully removed.", id);
                return _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DeleteStudentAsync: An error occurred while deleting the student with ID {StudentId}.", id);
                throw;
            }
        }

        public Task<IEnumerable<StudentInformationDTO>> GetAllStudentsAsync()
        {
            return Task.FromResult<IEnumerable<StudentInformationDTO>>(_context.StudentInformations.Select(
                x => new StudentInformationDTO
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    MobileNumber = x.MobileNumber,
                    Email = x.Email,
                    NicNumber = x.NicNumber
                }
                ).ToList());
        }

        public Task<StudentInformationDTO> GetStudentByIdAsync(int id)
        {
            var result = _context.StudentInformations.Find(id);
            if (result == null)
            {
                return Task.FromResult<StudentInformationDTO>(null);
            }

            var studentDetails = new StudentInformationDTO
            {
                Id = result.Id,
                FirstName = result.FirstName,
                LastName = result.LastName,
                MobileNumber = result.MobileNumber,
                Email = result.Email,
                NicNumber = result.NicNumber,
                DateOfBirth = result.DateOfBirth,
                Address = result.Address,
                ProfilePhoto = result.ProfilePhoto
            };

            return Task.FromResult(studentDetails);
        }

        public Task UpdateStudentAsync(StudentInformation student)
        {
            try
            {
                var existingStudent = _context.StudentInformations.Find(student.Id);
                if (existingStudent != null)
                {
                    existingStudent.FirstName = student.FirstName;
                    existingStudent.LastName = student.LastName;
                    existingStudent.MobileNumber = student.MobileNumber;
                    existingStudent.Email = student.Email;
                    existingStudent.NicNumber = student.NicNumber;
                    existingStudent.DateOfBirth = student.DateOfBirth;
                    _context.StudentInformations.Update(existingStudent);
                }
                else
                {
                    Log.Warning("UpdateStudentAsync: Student with ID {StudentId} not found.", student.Id);
                }
                return _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "UpdateStudentAsync: An error occurred while updating the student with ID {StudentId}.", student.Id);
                throw;
            }
        }

        public async Task<(List<StudentInformationDTO>, int)> GetPaginatedStudentsAsync(PaginationParameters pagination)
        {
            var query = _context.StudentInformations.AsQueryable();

            var totalCount = await query.CountAsync();

            var pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;
            var pageSize = pagination.PageSize < 1 ? 10 : pagination.PageSize;
            if (pageSize > 50) pageSize = 50;

            var students = await query
                .OrderBy(s => s.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new StudentInformationDTO
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    MobileNumber = s.MobileNumber,
                    Email = s.Email,
                    NicNumber = s.NicNumber,
                    DateOfBirth = s.DateOfBirth,
                    ProfilePhoto = s.ProfilePhoto,
                    Address = s.Address
                })
                .ToListAsync();

            return (students, totalCount);
        }
    }
}
