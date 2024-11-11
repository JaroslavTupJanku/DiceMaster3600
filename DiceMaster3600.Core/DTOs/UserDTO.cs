using DiceMaster3600.Core.Enum;

namespace DiceMaster3600.Core.DTOs
{
    public class UserDTO
    {
        public int NumberOfPoints { get; set; }
        public Gender Gender { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public UserDTO() { }

        public UserDTO(string name, string surname, string email, Gender gender, int nbPoints = 0)
        {
            Name = name;
            Surname = surname;
            EmailAddress = email;
            Gender = gender;
            NumberOfPoints = nbPoints;
        }
    }
}
