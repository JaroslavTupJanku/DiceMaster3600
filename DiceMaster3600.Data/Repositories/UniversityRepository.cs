using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Data.Entitites;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiceMaster3600.Data.Repositories
{
    public class UniversityRepository : Repository<UniversityEntity>
    {
        public UniversityRepository(SqlEFDataContext context) : base(context) { }

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
