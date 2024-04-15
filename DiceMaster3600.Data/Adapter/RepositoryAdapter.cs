using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Data.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiceMaster3600.Data.Adapter
{
    public class RepositoryAdapter
    {
        #region Fields
        private readonly ISqlRepositories repos;
        #endregion

        #region Constructors
        public RepositoryAdapter(ISqlRepositories repositories) => repos = repositories;
        #endregion

        #region Methods
        public async Task DeleteUniversityById(int UniversityID)
        {
            using var transaction = repos.BeginTransaction();
            try
            {
                var university = repos.UniversityRepository.GetById(UniversityID)
                    ?? throw new KeyNotFoundException($"University with ID {UniversityID} was not found.");

                foreach (var faculty in university.Faculties.ToList())
                {
                    await repos.UserRepository.DeleteAllByFacultyIdAsync(faculty.Id);
                    await repos.FacultyRepository.DeleteAsync(faculty);
                }

                await repos.UniversityRepository.DeleteAsync(university);
                await transaction.CommitAsync();
            }
            catch ( Exception ex )
            {
                transaction.Rollback();
                throw new ApplicationException($"Failed to delete university with ID {UniversityID}: {ex.Message}", ex);
            }
        }

        public async Task RegisterUser(UserDTO userDto, string plainPassword, UniversityType universityType, FacultyType facultyType)
        {
            var universityEntity = await repos.UniversityRepository.EnsureUniversityExistsAsync(universityType);
            var facultyEntity = await repos.FacultyRepository.EnsureFacultyExistsAsync(facultyType, universityEntity.Id);

            await repos.UserRepository.AddAsync(userDto, plainPassword, facultyEntity.Id);
        }

        public async Task<UserDTO?> GetUserByEmailAsync(string email, string plainPassword)
        {
            var userEntity = await repos.UserRepository.GetUserByEmailAsync(email, plainPassword);

            return userEntity is UserEntity e ? new UserDTO()
            {
                Name = e.Name,
                Surname = e.Surname,
                EmailAddress = e.EmailAddress,
                Gender = e.Gender,
                NumberOfPoints = e.NumberOfPoints
            } : null;
        }

        public async Task<UniversityDTO> GetUniversityDTOByIdAsync(int selectedUniversityId)
        {
            var universityDTO = repos.UniversityRepository.GetDTOById(selectedUniversityId);
            universityDTO.Faculties =  await repos.FacultyRepository.GetFacultyDtoByUniversityIDAsync(selectedUniversityId);

            return universityDTO;
        }

        public async Task<UserDTO[]> GetTopThree() => await repos.UserRepository.GetTopThreePlayersAsync();
        #endregion

        #region Events

        #endregion
    }
}
