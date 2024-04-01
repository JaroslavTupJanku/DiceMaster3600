using DiceMaster3600.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
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
        private readonly SqlEFDataContext context = new();

        private readonly Lazy<FacultyRepository> facultyRepository;
        private readonly Lazy<UserRepository> userRepository;
        private readonly Lazy<UniversityRepository> universityRepository;
        #endregion

        #region Property
        public FacultyRepository FacultyRepository => facultyRepository.Value;
        public UserRepository UserRepository => userRepository.Value;
        public UniversityRepository UniversityRepository => universityRepository.Value;
        #endregion

        #region Constructors
        public SqlRepositories()
        {
            facultyRepository = new Lazy<FacultyRepository>(() => new FacultyRepository(context));
            userRepository = new Lazy<UserRepository>(() => new UserRepository(context));
            universityRepository = new Lazy<UniversityRepository>(() => new UniversityRepository(context));
        }
        #endregion

        #region Methods
        public IDbContextTransaction BeginTransaction() => context.Database.BeginTransaction();
        public void Dispose() => context.Dispose();
        #endregion

        #region Events

        #endregion
    }
}
