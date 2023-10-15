using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.Core.DTOs
{
    public class UserDTO
    {
        public int Id { get; }
        public int NumberOfPoints { get;}
        public string Name { get; } = string.Empty;
        public string Surname { get; } = string.Empty;
        public string EmailAddress { get; } = string.Empty;

        public UniversityDTO University { get; set; } = null!;
        public FacultyDTO Faculty { get; set; } = null!;
    }
}
