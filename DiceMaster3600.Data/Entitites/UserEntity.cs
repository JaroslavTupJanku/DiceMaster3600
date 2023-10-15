namespace DiceMaster3600.Data.Entitites
{
    public class UserEntity
    {
        public int Id { get; set; }

        public int NumberOfPoints { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;

        public int UniversityId { get; set; }
        public UniversityEntity University { get; set; } = null!;

        public int FacultyId { get; set; }  
        public FacultyEntity Faculty { get; set; } = null!;
    }
}
