using ContactsManager.Core.Enumerator;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace ContactsManager.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name cant't be blank")]
        public string? Name{ get; set; }

        [Required(ErrorMessage = "Email cant't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        [DataType(DataType.EmailAddress)]
        [Remote(action:"IsEmailAlreadyRegistered" , controller:"Account" , ErrorMessage ="Email Alread use")]
        public string? Email{ get; set; }

        [Required(ErrorMessage = "Email cant't be blank")]
        [RegularExpression("^[0-9]*$",ErrorMessage ="phone number should contain number only")]
        public string? Phone { get; set; }

        [Required(ErrorMessage ="password can't be blank")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }

        public UserTypeOptions UserOption { get; set; } = UserTypeOptions.User;
    }
}
