using DiceMaster3600.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.Data.Entitites
{
    public class FacultyEntity
    {
        public int Id { get; set; }
        public FacultyType Name { get; set; }

        public int UniversityId { get; set; }
        public UniversityEntity University { get; set; } = null!;
    }
}
