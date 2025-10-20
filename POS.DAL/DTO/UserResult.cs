using System.Collections.Generic;

namespace POS.DAL.DTO
{
    public class UserResult
    {
        public IEnumerable<UserDTO> Users { get; set; }
    }
}
