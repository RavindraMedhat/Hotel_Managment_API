using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_Managment_API.Models
{
    public class User
    {
        [Required]
        [Key]
        public string EmailID { get; set; }
        [Required]

        public string Password { get; set; }
        [Required]
        [ForeignKey("User_ID")]
        public int User_ID { get; set; }
    }
}
