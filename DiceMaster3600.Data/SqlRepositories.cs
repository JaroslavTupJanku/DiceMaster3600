using DiceMaster3600.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.Data
{
    public class SqlRepositories : IDisposable, ISqlRepositories
    {
        #region Fields
        private readonly SqlEFDataContext context = new SqlEFDataContext();
        #endregion

        #region Property
        public FacultyRepository FacultyRepository { get; set; }
        public UserRepository UserRepository { get; set; }
        public UniversityRepository UniversityRepository { get; set; }
        #endregion

        #region Constructors
        public SqlRepositories()
        {
            FacultyRepository = new FacultyRepository(context);
            UserRepository = new UserRepository(context);
            UniversityRepository = new UniversityRepository(context);
        }
        #endregion

        #region Methods
        public void Dispose() => context.Dispose();
        #endregion

        #region Events

        #endregion
    }
}
