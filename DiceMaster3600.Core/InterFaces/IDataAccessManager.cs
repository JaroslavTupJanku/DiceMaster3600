using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.Enum;
using System;
using System.Collections.Generic;

namespace DiceMaster3600.Core.InterFaces
{
    public interface IDataAccessManager
    {
        List<UserDTO> GetTopThreePlayers();
        bool AddUser(UserDTO user, UniversityType univeristy, FacultyType faculty);

        void DeleteUniversity(int id);
        UniversityDTO[] GetAllUniversityDTOs();
        UniversityDTO GetUniversityByID(int id);


        public event EventHandler? OnDatabaseUpdated;
    }
}
