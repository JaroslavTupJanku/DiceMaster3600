using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.Core.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public int NumberOfPoints { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;  
    }
}
