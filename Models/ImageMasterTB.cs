﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_Management.Models
{
    public class ImageMasterTB
    {
        [Key]
        public int Image_ID { get; set; }
        [Required]
        public string Image_URl { get; set; }
        [Required]
        public int Reference_ID { get; set; }
        [Required]
        public string ReferenceTB_Name { get; set; }
        [Required]
        public bool Active_Flag { get; set; }
        [Required]
        public bool Delete_Flag { get; set; }
        [Required]
        public float sortedfield { get; set; }
    }
}
