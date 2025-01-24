using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[A-Za-zĄąĆćĘęŁłŃńÓóŚśŹźŻż]+(?:[\s-][A-Za-zĄąĆćĘęŁłŃńÓóŚśŹźŻż]+)*$", ErrorMessage = "Name must contain only letters and spaces.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
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

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[*.!@$%^&:.?~_+-=]).{8,32}$",
ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one number and special char.")]
        public string Password { get; set; }

        public RegisterViewModel() { }
        public RegisterViewModel(string name, string surname, string email, string phone, int age, string password)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Phone = phone;
            Age = age;
            Password = password;
        }

    }
}
