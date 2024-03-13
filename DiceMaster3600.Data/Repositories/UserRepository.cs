using DiceMaster3600.Data.Entitites;
using System.Data.SqlTypes;
using System.Linq;

namespace DiceMaster3600.Data.Repositories
{
    public class UserRepository : Repository<UserEntity>
    {
        public UserRepository(SqlEFDataContext context) : base(context) { }

        public void DeleteByFacultyId(int facultyID)
        {
            var entities = context.Users.Where(x => x.FacultyId == facultyID && x.DeletedDate == null)?.ToArray()
                ?? throw new SqlNullValueException($"Faculty id {facultyID} does not exist in the database");

            Delete(entities);
            context.SaveChanges();
        }
    }
}
