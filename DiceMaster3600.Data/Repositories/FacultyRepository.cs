using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Data.Entitites;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.Metrics;
using System.Linq;

namespace DiceMaster3600.Data.Repositories
{
    public class FacultyRepository : Repository<FacultyEntity>
    {
        public FacultyRepository(SqlEFDataContext context) : base(context) { }

        public List<FacultyDTO> GetFacultyDtoByUniversityID(int UniversityID)
        {
            var dtos = new List<FacultyDTO>();
            var entities = context.Faculties.Where(x => x.UniversityId == UniversityID && x.DeletedDate == null).ToList();

            foreach (var facultyEntity in entities)
            {
                var userEntities = context.Users.Where(x => x.FacultyId == facultyEntity.Id).ToList();
                dtos.Add(new FacultyDTO()
                {
                    Name = facultyEntity.Name,
                    Users = userEntities.Select(y => new UserDTO()
                    {
                        Id = y.Id,
                        EmailAddress = y.EmailAddress,
                        Name = y.Name,
                        Surname = y.Surname,
                        NumberOfPoints = y.NumberOfPoints,
                    }).ToList()
                });
            }
            return dtos;
        }

        public void DeleteFacultyByUniversityID(int UniversityID)
        {
            var entities = context.Faculties.Where(x => x.UniversityId == UniversityID && x.DeletedDate == null)?.ToArray()
                ?? throw new SqlNullValueException($"University id {UniversityID} does not exist in the database");

            Delete(entities);
            context.SaveChanges();  
        }
    }
}
