﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WISProject1.DAL;


namespace WISProject1.Models.Home
{
    public class Item
    {
        public Tbl_Product Product { get; set; }
        public int Quantity { get; set; }
    }
}