using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.Enum;
using System;
using System.Threading.Tasks;

namespace DiceMaster3600.Core.InterFaces
{
    public interface IDataAccessManager
    {
        Task<RankedUserDTO[]> GetTopThreePlayersAsync();
        Task<UniversityRankingDTO[]> GetTopThreeUniversityAsync();

        Task RegisterUserAsync(UserDTO user, UniversityType univeristy, FacultyType faculty);
        Task DeleteUniversityAsync(int id);
        UniversityDTO[] GetAllUniversityDTOs();
        Task GetUniversityByID(int id);
        Task<UserDTO?> GetUserByEmailAsync(string email);

        public event Action<bool>? OnProcessingStatusChanged;
        public event EventHandler? OnDatabaseUpdated;
    }
}
