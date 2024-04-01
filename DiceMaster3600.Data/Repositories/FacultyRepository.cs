using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Data.Entitites;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiceMaster3600.Data.Repositories
{
    public class FacultyRepository : Repository<FacultyEntity>
    {
        public FacultyRepository(SqlEFDataContext context) : base(context) { }

        public List<FacultyDTO> GetFacultyDtoByUniversityID(int universityID)
        {
            var faculties = context.Faculties
                .Where(f => f.UniversityId == universityID && f.DeletedDate == null)
                .Include(f => f.Users)
                .ToList();

            var dtos = faculties.Select(faculty => new FacultyDTO
            {
                Name = faculty.Name,
                Users = faculty.Users.Select(user => new UserDTO
                {
                    EmailAddress = user.EmailAddress,
                    Name = user.Name,
                    Surname = user.Surname,
                    NumberOfPoints = user.NumberOfPoints,
                }).ToList()
            }).ToList();

            return dtos;
        }

        public async Task<FacultyEntity> AddAsync(FacultyDTO facultyDTO, int universityId)
        {
            var facultyEntity = new FacultyEntity
            {
                Name = facultyDTO.Name,
                UniversityId = universityId
            };

            await context.Faculties.AddAsync(facultyEntity);
            await context.SaveChangesAsync();
            return facultyEntity;
        }

        public void DeleteFacultyByUniversityID(int universityID)
        {
            var entities = context.Faculties.Where(x => x.UniversityId == universityID && x.DeletedDate == null).ToArray()
                ?? throw new KeyNotFoundException($"University id {universityID} does not exist in the database");

            Delete(entities);
            context.SaveChanges();
        }

        public async Task<FacultyEntity?> FindByNameAndUniversityIdAsync(FacultyType name, int universityID)
        {
            return await context.Faculties.FirstOrDefaultAsync(f => f.Name == name
                                            && f.UniversityId == universityID
                                            && f.DeletedDate == null);
        }

        public async Task<FacultyEntity> EnsureFacultyExistsAsync(FacultyType name, int universityId)
        {
            var faculty = await FindByNameAndUniversityIdAsync(name, universityId);
            faculty ??= await AddAsync(new FacultyDTO { Name = name }, universityId);

            return faculty;
        }
    }
}
