using DiceMaster3600.Core.Enum;
using System;
using System.Collections.Generic;

namespace DiceMaster3600.Data.Entitites
{
    public class UniversityEntity : ISoftDeleteEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime? DeletedDate { get; set; }
        public UniversityType Name { get; set; }

        public ICollection<FacultyEntity> Faculties { get; set; } = null!;
    }
}
