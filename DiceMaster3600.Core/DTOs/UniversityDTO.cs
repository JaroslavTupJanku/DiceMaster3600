using DiceMaster3600.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.Core.DTOs
{
    public class UniversityDTO
    {
        public int Id { get; }
        public UniversityType Name { get; }

        public IList<FacultyDTO> Pizzas { get; } = null!;
    }
}
