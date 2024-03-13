using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Data.Entitites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.Data.Repositories
{
    public class UniversityRepository : Repository<UniversityEntity>
    {
        public UniversityRepository(SqlEFDataContext context) : base(context) { }

        public UniversityDTO GetDTOById(int id)
        {
            var item = GetById(id) ?? throw new SqlNullValueException("No university was found");
            return new UniversityDTO()
            {
                Id = item.Id,
                Name = item.Name,   
            };
        }
    }
}
