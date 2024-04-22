using DiceMaster3600.Core.Enum;

namespace DiceMaster3600.Core.DTOs
{
    public class UniversityRankingDTO
    {
        public UniversityType UniversityName { get; set; }
        public double AveragePoints { get; set; }
        public int StudentCount { get; set; }

    }
}
