using DiceMaster3600.Core.Enum;
using System.Collections.Generic;

namespace DiceMaster3600.Core.DTOs
{
    public class FacultyDTO
    {
        public int Id { get; set; }
        public List<UserDTO> Users { get; set; } = new();
        public FacultyType Name { get; set; }

    }
}
