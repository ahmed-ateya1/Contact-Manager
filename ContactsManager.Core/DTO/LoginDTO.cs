using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="email can't be blank")]
        [EmailAddress(ErrorMessage = "email should be in a proper email address format")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage ="password can't be blank")]

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe {  get; set; } = false;
    }
}
