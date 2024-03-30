using DiceMaster3600.Core.DTOs;
using System.Data.SqlTypes;

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
        public void DeleteUniversityById(int id)
        {
            var university = repos.UniversityRepository.GetById(id)
                ?? throw new SqlNullValueException($"University id {id} was not found");

            foreach (var faculty in university.Faculties)
            {
                repos.UserRepository.DeleteByFacultyId(faculty.Id);
                repos.FacultyRepository.Delete(faculty);
            }

            repos.UniversityRepository.Delete(university);
        }
        #endregion

        #region Methods
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
