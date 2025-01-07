using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Range(14, 150)]
        public int Age { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [NotMapped]
        public static PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

        public User(string name, string surname, string email, string phone, int age, string passwordHash)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Phone = phone;
            Age = age;
            PasswordHash = passwordHash;
        }

        public User(RegisterViewModel model)
        {
            Name = model.Name;
            Surname = model.Surname;
            Email = model.Email;
            Phone = model.Phone;
            Age = model.Age;
            PasswordHash = passwordHasher.HashPassword(this, model.Password);
        }

        public User(UserProfileViewModel model, User currentUser)
        {
            Id = currentUser.Id;
            Name = model.Name;
            Surname = model.Surname;
            Email = model.Email;
            Phone = model.Phone;
            Age = model.Age;
            PasswordHash = model.NewPassword is null ? currentUser.PasswordHash : passwordHasher.HashPassword(this, model.NewPassword);
        }
    }
}
