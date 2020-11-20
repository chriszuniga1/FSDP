using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ZipNTuck.UI.MVC.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "*Please enter a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*Please include an e-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "*Please include a message")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "*Please include a message")]
        [UIHint("MultilineText")]
        public string Message { get; set; }
    }
}