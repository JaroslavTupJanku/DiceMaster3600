using DiceMaster3600.Core.Enum;
using System.Collections.Generic;

namespace DiceMaster3600.Core.DTOs
{
    public class UniversityDTO
    {
        public int Id { get; set; }
        public UniversityType Name { get; set; }

        public List<FacultyDTO> Faculties { get; set; } = null!;
    }
}
