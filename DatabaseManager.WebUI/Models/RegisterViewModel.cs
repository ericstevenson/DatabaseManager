using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DatabaseManager.WebUI.Models
{
    public class RegisterViewModel
    {
        [Required]
        [RegularExpression(@"^[0-9A-Za-z]{5,20}$", ErrorMessage = "Invalid Username. Usernames can only contain letters and numbers, and must be between 5 and 20 characters.")]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{5,15}$", ErrorMessage = "Invalid Password. Must be between 5 and 15 characters long and must contain at least one number, one uppercase letter, and one lowercase letter.")]
        public string Password { get; set; }

        [Required]
        [Display(Name="Re-enter Password")]
        public string ReEnteredPassword { get; set; }
    }
}