using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Entities;

namespace BookLibrary.Models.Books
{
    public class EditVM : CreateVM
    {
        public int Id { get; set; }
    }
}
