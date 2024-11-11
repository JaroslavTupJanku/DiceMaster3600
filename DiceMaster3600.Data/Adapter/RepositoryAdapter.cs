using DiceMaster3600.Core.DTOs;
using System.Data.SqlTypes;
using System.Threading.Tasks;

namespace DiceMaster3600.Data.Adapter
{
    public class RepositoryAdapter
    {
        #region Fields
        private readonly SqlRepositories repos;
        #endregion

        #region Constructors
        public RepositoryAdapter(SqlRepositories repositories) => repos = repositories;
        #endregion

        #region Methods
        public async void DeleteUniversityById(int id)
        {
            var university = repos.UniversityRepository.GetById(id)
                ?? throw new SqlNullValueException($"University id {id} was not found");

            foreach (var faculty in university.Faculties)
            {
                await repos.UserRepository.DeleteAllByFacultyIdAsync(faculty.Id);
                await repos.FacultyRepository.DeleteAsync(faculty);
            }

           await repos.UniversityRepository.DeleteAsync(university);
        }
        #endregion

        #region Methods
        public async Task<UniversityDTO> GetUniversityDTOByIdAsync(int selectedUniversityId)
        {
            var universityDTO = repos.UniversityRepository.GetDTOById(selectedUniversityId);
            universityDTO.Faculties = await repos.FacultyRepository.GetFacultyDtoByUniversityIDAsync(selectedUniversityId);

            return universityDTO;
        }
        #endregion

        #region Events

        #endregion
    }
}
