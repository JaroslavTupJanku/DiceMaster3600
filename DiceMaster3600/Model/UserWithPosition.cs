using DiceMaster3600.Core.DTOs;
using System.Xml.Linq;

namespace DiceMaster3600.Model
{
    public class UserWithPosition //Wrapper
    {
        public string WholeName { get; }
        public string RelativePoints { get; }
        public int NumberOfPoints { get; }
        public int Position { get; }

        public UserWithPosition(UserDTO user, int position)
        {
            WholeName = $"{user.Name} {user.Surname}";
            RelativePoints = $"{user.NumberOfPoints}/3600";
            NumberOfPoints = user.NumberOfPoints;
            Position = position;
        }
    }
}
