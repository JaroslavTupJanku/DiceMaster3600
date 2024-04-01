using DiceMaster3600.Core.DTOs;
using System;
using System.Collections.Generic;

namespace DiceMaster3600.Core.InterFaces
{
    public interface IDataAccessManager
    {
        public void DeleteUniversity(int id);
        List<UserDTO> GetTopThreePlayers();

        public UniversityDTO[] GetAllUniversityDTOs();
        public UniversityDTO GetUniversityByID(int id);


        public event EventHandler? OnDatabaseUpdated;
    }
}
