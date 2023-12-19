﻿using Hotel_Managment_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_Managment_API.ViewModels
{
    public class BillRes
    {
        public Billing bill { get; set; }
        public List<Details> details { get; set; }
    }

    public class Details
    {
        public string detail { get; set; }
        public float amount { get; set; }
    }
}
