using System.ComponentModel.DataAnnotations;

namespace StudentRegistrationSystemApi.Model.DTO
{
    public class StudentInformationDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        public required string LastName { get; set; }

        [Required]
        [Phone(ErrorMessage = "Invalid mobile number format.")]
        [StringLength(20, ErrorMessage = "Mobile number cannot be longer than 20 characters.")]
        public required string MobileNumber { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public required string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "NIC number cannot be longer than 20 characters.")]
        public required string NicNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [StringLength(255, ErrorMessage = "Profile photo path cannot be longer than 255 characters.")]
        public string? ProfilePhoto { get; set; }

        public IFormFile? FileInfo { get; set; }

        public string? Address { get; set; }
    }


    public class PaginationParameters
    {
        private int _pageSize = 10;
        private const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }

    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
