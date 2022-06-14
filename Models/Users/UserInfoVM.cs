using BookLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models.Users
{
    public class UserInfoVM : CreateVM
    {
        public int Id { get; set; }
        public List<RentedBook> RentBooks{ get; set; }
    }
}
