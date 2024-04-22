using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Data.Entitites;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiceMaster3600.Data.Repositories
{
    public class UniversityRepository : Repository<UniversityEntity>
    {
        public UniversityRepository(SqlEFDataContext context) : base(context) { }

        public async Task<UniversityRankingDTO[]> GetTopThreeUniversitiesAsync()
        {
            return await context.Universities
                .Where(university => university.DeletedDate == null)
                .Select(university => new {
                    University = university,
                    Faculties = university.Faculties
                        .Where(faculty => faculty.DeletedDate == null)
                        .SelectMany(faculty => faculty.Users)
                        .Where(user => user.DeletedDate == null)
                })
                .Select(data => new UniversityRankingDTO {
                    UniversityName = data.University.Name,
                    AveragePoints = data.Faculties.Any() ? data.Faculties.Average(user => user.NumberOfPoints) : 0,
                    StudentCount = data.Faculties.Count()
                })
                .OrderByDescending(dto => dto.AveragePoints)
                .ThenByDescending(dto => dto.StudentCount)
                .Take(3).ToArrayAsync();
        }

        public UniversityDTO GetDTOById(int id)
        {
            var item = GetById(id) ?? throw new KeyNotFoundException("No university was found");
            return new UniversityDTO()
            {
                Id = item.Id,
                Name = item.Name,
            };
        }

        public async Task<UniversityEntity> AddAsync(UniversityDTO universityDTO)
        {
            var universityEntity = new UniversityEntity
            {
                Name = universityDTO.Name
            };

            await context.Universities.AddAsync(universityEntity);
            await context.SaveChangesAsync();
            return universityEntity;
        }

        public async Task<UniversityEntity?> FindByNameAsync(UniversityType name)
        {
            return await context.Universities.FirstOrDefaultAsync(u => u.Name == name && u.DeletedDate == null);
        }

        public async Task<UniversityEntity> EnsureUniversityExistsAsync(UniversityType name)
        {
            return await FindByNameAsync(name) ?? await AddAsync(new UniversityDTO { Name = name });
        }
    }
}
