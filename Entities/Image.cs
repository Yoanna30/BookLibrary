﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace BookLibrary.Entities
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }

    }
}
