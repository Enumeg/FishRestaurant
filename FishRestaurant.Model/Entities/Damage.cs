﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishRestaurant.Model.Entities
{
    public class Damage
    {      
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amonut { get; set; }
    }
}
