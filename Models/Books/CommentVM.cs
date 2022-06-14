using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Entities;

namespace BookLibrary.Models.Books
{
    public class CommentVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }

    }
}
