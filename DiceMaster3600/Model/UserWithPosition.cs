using DiceMaster3600.Core.DTOs;

namespace DiceMaster3600.Model
{
    public class UserWithPosition //Wrapper
    {
        public UserDTO? User { get; set; }
        public int Position { get; set; }
    }
}
