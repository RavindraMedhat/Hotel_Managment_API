﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_Managment_API.ViewModels
{
    public class ReqSendCoupen
    {
        public List<int> userIds { get; set; }
        public string Cname { get; set; }
        public int hid { get; set; }
    }
}
