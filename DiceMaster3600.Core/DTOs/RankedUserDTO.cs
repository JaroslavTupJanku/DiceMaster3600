using DiceMaster3600.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.Core.DTOs
{
    public class RankedUserDTO
    {
        public string WholeName { get; private set; }
        public string RelativePoints { get; private set; }
        public int NumberOfPoints { get; private set; }
        public int Position { get; private set; }
        public Gender Gender { get; private set; }  
        public UniversityType University{ get; private set; } 

        public RankedUserDTO(UserDTO user, int position)
        {
            WholeName = $"{user.Name} {user.Surname}";
            NumberOfPoints = user.NumberOfPoints;
            Position = position;
            RelativePoints = $"{user.NumberOfPoints / 3600 * 100}%";
            Gender = user.Gender; 
        }

    }
}
