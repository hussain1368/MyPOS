using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DAL.Models
{
    public class UserDTM
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string UserRole { get; set; }
        public bool IsDeleted { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
