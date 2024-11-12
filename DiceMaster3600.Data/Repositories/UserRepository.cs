using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Data.Entitites;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DiceMaster3600.Data.Repositories
{
    public class UserRepository : Repository<UserEntity>
    {
        public UserRepository(SqlEFDataContext context) : base(context) { }

        public async Task DeleteAllByFacultyIdAsync(int facultyID)
        {
            var entities = await context.Users
                .Where(x => x.FacultyId == facultyID && x.DeletedDate == null)
                .ToListAsync();

            if (entities.Any())
                await DeleteAsync(entities);
        }

        public async Task<UserDTO[]> GetTopThreePlayersAsync()
        {
            var topThreeUsers = await context.Users
                .Where(user => user.DeletedDate == null)
                .OrderByDescending(user => user.NumberOfPoints)
                .Take(3).ToListAsync();

            return topThreeUsers.Select(user => new UserDTO
            {
                Name = user.Name,
                Surname = user.Surname,
                EmailAddress = user.EmailAddress,
                Gender = user.Gender,
                NumberOfPoints = user.NumberOfPoints
            }).ToArray();
        }

        public async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            return await context.Users.FirstOrDefaultAsync(u 
                => u.EmailAddress == email 
                && u.DeletedDate == null);
        }

        public async Task AddAsync(UserDTO userDTO, int facultyId)
        {
            var userEntity = new UserEntity
            {
                Name = userDTO.Name,
                Surname = userDTO.Surname,
                Gender = userDTO.Gender,
                FacultyId = facultyId,
                EmailAddress = userDTO.EmailAddress,
                NumberOfPoints = userDTO.NumberOfPoints,
            };

            await context.Users.AddAsync(userEntity);
            await context.SaveChangesAsync();
        }
    }
}
