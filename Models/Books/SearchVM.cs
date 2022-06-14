using BookLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models.Books
{
    public class SearchVM:DetailsVM
    {
        public List<Book> bookCollect { get; set; }
    }
}
