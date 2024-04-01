using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Data.Entitites;
using System.Linq;
using System.Threading.Tasks;

namespace DiceMaster3600.Data.Repositories
{
    public class UserRepository : Repository<UserEntity>
    {
        public UserRepository(SqlEFDataContext context) : base(context) { }

        public void DeleteAllByFacultyId(int facultyID)
        {
            var entities = context.Users.Where(x => x.FacultyId == facultyID && x.DeletedDate == null).ToArray();

            if (entities.Any())
            {
                Delete(entities);
                context.SaveChanges();
            }
        }

        public async Task AddAsync(UserDTO userDTO, int facultyId)
        {
            var userEntity = new UserEntity
            {
                Name = userDTO.Name,
                Surname = userDTO.Surname,
                FacultyId = facultyId,
                EmailAddress = userDTO.EmailAddress,
                NumberOfPoints = userDTO.NumberOfPoints,
            };

            await context.Users.AddAsync(userEntity);
            await context.SaveChangesAsync();
        }

    }
}
