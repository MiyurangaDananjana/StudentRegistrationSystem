using StudentRegistrationSystemApi.Model.DTO;
using StudentRegistrationSystemApi.Model.Entity;

namespace StudentRegistrationSystemApi.Data.Repositories.Interfaces
{
    public interface IStudentsRepositories
    {
        Task<IEnumerable<StudentInformationDTO>> GetAllStudentsAsync();
        Task<StudentInformationDTO> GetStudentByIdAsync(int id);
        Task AddStudentAsync(StudentInformation student);
        Task UpdateStudentAsync(StudentInformation student);
        Task DeleteStudentAsync(int id);
        Task<(List<StudentInformationDTO> Students, int TotalCount)> GetPaginatedStudentsAsync(PaginationParameters pagination);
    }
}
