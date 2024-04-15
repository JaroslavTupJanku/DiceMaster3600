using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.Enum;
using System;
using System.Threading.Tasks;

namespace DiceMaster3600.Core.InterFaces
{
    public interface IDataAccessManager
    {
        Task<UserDTO[]> GetTopThreePlayersAsync();
        Task RegisterUserAsync(UserDTO user, string plainPassword, UniversityType univeristy, FacultyType faculty);
        Task DeleteUniversityAsync(int id);
        UniversityDTO[] GetAllUniversityDTOs();
        Task GetUniversityByID(int id);
        Task<UserDTO?> GetUserByEmailAsync(string email, string plainPassword);

        public event Action<bool>? OnProcessingDataChanged;
        public event EventHandler? OnDatabaseUpdated;
    }
}
