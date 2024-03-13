using DiceMaster3600.Core.Enum;
using System;
using System.Collections.Generic;

namespace DiceMaster3600.Data.Entitites
{
    public class FacultyEntity : ISoftDeleteEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime? DeletedDate { get; set; }
        public FacultyType Name { get; set; }

        public int UniversityId { get; set; }
        public UniversityEntity University { get; set; } = null!;

        public ICollection<UserEntity> Users { get; set; } = null!;
    }
}
