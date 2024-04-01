using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.Enum;
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
        public void DeleteUniversityById(int UniversityID)
        {
            using var transaction = repos.BeginTransaction();
            try
            {
                var university = repos.UniversityRepository.GetById(UniversityID)
                    ?? throw new KeyNotFoundException($"University with ID {UniversityID} was not found.");

                foreach (var faculty in university.Faculties.ToList())
                {
                    repos.UserRepository.DeleteAllByFacultyId(faculty.Id);
                    repos.FacultyRepository.Delete(faculty);
                }

                repos.UniversityRepository.Delete(university);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task AddUser(UserDTO userDto, UniversityType universityType, FacultyType facultyType)
        {
            var universityEntity = await repos.UniversityRepository.EnsureUniversityExistsAsync(universityType);
            var facultyEntity = await repos.FacultyRepository.EnsureFacultyExistsAsync(facultyType, universityEntity.Id);

            await repos.UserRepository.AddAsync(userDto, facultyEntity.Id);
        }

        public UniversityDTO GetUniversityDTOById(int selectedUniversityId)
        {
            var universityDTO = repos.UniversityRepository.GetDTOById(selectedUniversityId);
            universityDTO.Faculties = repos.FacultyRepository.GetFacultyDtoByUniversityID(selectedUniversityId);

            return universityDTO;
        }
        #endregion

        #region Events

        #endregion
    }
}
