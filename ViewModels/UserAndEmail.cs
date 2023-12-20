﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_Managment_API.ViewModels
{
    public class UserAndEmail
    {
        [Key]
        public int User_ID { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Name can not more than 50 character")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "Enter Valid Email")]
        public string Email { get; set; }
    }
}
