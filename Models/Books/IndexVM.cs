using BookLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models.Books
{
    public class IndexVM
    {
        public List<Book> bookCollection { get; set; }
        public List<Summary> Summary { get; set; }
    }
}
