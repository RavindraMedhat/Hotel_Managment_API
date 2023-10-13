﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_Managment_API.ViewModels
{
    public class HotelViewModelForIndex
    {
        [Key]
        public int Hotel_ID { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Name can not more than 50 character")]
        public string Hotel_Name { get; set; }

        public List<String> Image_URl { get; set; }
    }
}
