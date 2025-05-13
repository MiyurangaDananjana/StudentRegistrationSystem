using System.ComponentModel.DataAnnotations;

namespace StudentRegistrationSystemApi.Model.Entity
{
    public class StudentInformation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required string MobileNumber { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string NicNumber { get; set; }

        [Required]
        public required DateTime DateOfBirth { get; set; }

        public  string? ProfilePhoto { get; set; }

        public DateTime CreateBy { get; set; } = DateTime.UtcNow;

        public string? Address { get; set; }

    }
}
