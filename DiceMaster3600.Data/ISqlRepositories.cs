using DiceMaster3600.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.Data
{
    public interface ISqlRepositories
    {
        FacultyRepository FacultyRepository { get; }
        UserRepository UserRepository { get; }
        UniversityRepository UniversityRepository { get; }

        IDbContextTransaction BeginTransaction();
    }
}
