﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_Managment_API.ViewModels
{
    public class reqDataForAvailability
    {
        [Required]
        public int Room_id { get; set; }
        [Required]
        public DateTime date { get; set; }
    }
}
