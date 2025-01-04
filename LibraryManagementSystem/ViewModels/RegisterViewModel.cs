using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name must be less than 50 characters.")]
        [RegularExpression(@"^[A-Za-zĄąĆćĘęŁłŃńÓóŚśŹźŻż]+(?:[\s-][A-Za-zĄąĆćĘęŁłŃńÓóŚśŹźŻż]+)*$", ErrorMessage = "Name must contain only letters and spaces.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        [StringLength(50, ErrorMessage = "Surname must be less than 50 characters.")]
        [RegularExpression(@"^[A-Za-zĄąĆćĘęŁłŃńÓóŚśŹźŻż]+(?:[-\s][A-Za-zĄąĆćĘęŁłŃńÓóŚśŹźŻż]+)*$", ErrorMessage = "Surname must contain only letters, spaces and hyphens.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(14, 150, ErrorMessage = "Age must be between 14 and 150.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Age must be a number.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

    }
}
