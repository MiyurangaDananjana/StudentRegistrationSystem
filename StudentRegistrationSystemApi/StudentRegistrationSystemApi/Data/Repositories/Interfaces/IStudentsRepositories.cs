using StudentRegistrationSystemApi.Model.Entity;

namespace StudentRegistrationSystemApi.Data.Repositories.Interfaces
{
    public interface IStudentsRepositories
    {
        // Define the methods that will be implemented in the StudentsService class
        Task<IEnumerable<StudentInformation>> GetAllStudentsAsync();
        Task<StudentInformation> GetStudentByIdAsync(int id);
        Task AddStudentAsync(StudentInformation student);
        Task UpdateStudentAsync(StudentInformation student);
        Task DeleteStudentAsync(int id);
    }
}
