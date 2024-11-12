using DiceMaster3600.Core.Enum;
using System;

namespace DiceMaster3600.Data.Entitites
{
    public class UserEntity : ISoftDeleteEntity, IEntity
    {
        public int Id { get; set; }

        public DateTime? DeletedDate { get; set; }
        public int NumberOfPoints { get; set; }
        public Gender Gender { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;

        public int FacultyId { get; set; }  
        public FacultyEntity Faculty { get; set; } = null!;
    }
}
