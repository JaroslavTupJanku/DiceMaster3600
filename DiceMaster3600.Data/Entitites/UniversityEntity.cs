using DiceMaster3600.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.Data.Entitites
{
    public class UniversityEntity
    {
        public int Id { get; set; }

        public UniversityType Name { get; set; } 
        public ICollection<FacultyEntity> Faculties { get; set; } = null!;
    }
}
