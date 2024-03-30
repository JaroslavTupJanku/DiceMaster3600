using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Data.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.RightsManagement;
using System.Windows;

namespace DiceMaster3600.Model
{
    public class DataAccessManager : IDataAccessManager
    {
        #region Fields
        private readonly RepositoryAdapter dbModel;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public DataAccessManager(RepositoryAdapter dbModel) //AppSettings settings
        {
            this.dbModel = dbModel;
        }
        #endregion

        #region Methods
        public List<UserDTO> GetTopThreePlayers()
        {
            try
            {
                var universities = GetAllUniversityDTOs();
                var allUsers = universities.SelectMany(u => u.Faculties).SelectMany(f => f.Users).ToList();
                var topThreePlayers = allUsers.OrderByDescending(u => u.NumberOfPoints).Take(3).ToList();

                return topThreePlayers;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(
                    $"An error occurred: {ex.Message}", 
                    "Error", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                
                return new List<UserDTO>();
            }
        }

        public void DeleteUniversity(int id)
        {
            dbModel.DeleteUniversityById(id);
            OnDatabaseUpdated?.Invoke(this, EventArgs.Empty);
        }

        public UniversityDTO[] GetAllUniversityDTOs()
        {
            var userArray1 = new UserDTO[] {
                new UserDTO() {Name = "Hana", Surname= "Černý", NumberOfPoints = 2675, EmailAddress="Email0@gmail.com"},
                new UserDTO() {Name = "Pavel", Surname= "Horák", NumberOfPoints = 2802, EmailAddress="Email8@gmail.com"},
                new UserDTO() {Name = "Pavel", Surname= "Dvořák", NumberOfPoints = 550, EmailAddress="Email3@gmail.com"},
            };

            var userArray2 = new UserDTO[] {
                new UserDTO() {Name = "Pavel", Surname= "Kučera", NumberOfPoints = 1804, EmailAddress="Email13@gmail.com"},
                new UserDTO() {Name = "Jan", Surname= "Kučera", NumberOfPoints = 79, EmailAddress="Email4@gmail.com"},
                new UserDTO() {Name = "Jan", Surname= "Kučera", NumberOfPoints = 1204, EmailAddress="Email11@gmail.com"},
            };

            var userArray3 = new UserDTO[] {
                new UserDTO() {Name = "Jan", Surname= "Novotný", NumberOfPoints = 1853, EmailAddress="Email7@gmail.com"},
                new UserDTO() {Name = "Marie", Surname= "Dvořák", NumberOfPoints = 265, EmailAddress="Email1@gmail.com"},
                new UserDTO() {Name = "Anna", Surname= "Krhutová", NumberOfPoints = 1960, EmailAddress="Email5@gmail.com"},
            };

            var userArray4 = new UserDTO[] {
                new UserDTO() {Name = "Martin", Surname= "Svoboda", NumberOfPoints = 3021, EmailAddress="Email2@gmail.com"},
                new UserDTO() {Name = "Petr", Surname= "Novotný", NumberOfPoints = 4249, EmailAddress="Email12@gmail.com"},
                new UserDTO() {Name = "Marie", Surname= "Novotný", NumberOfPoints = 1102, EmailAddress="Email10@gmail.com"},
            };

            var userArray5 = new UserDTO[] {
                new UserDTO() {Name = "Lucie", Surname= "Procházka", NumberOfPoints = 2842, EmailAddress="Email9@gmail.com"},
                new UserDTO() {Name = "Marie", Surname= "Novotný", NumberOfPoints = 2165, EmailAddress="Email6@gmail.com"},
                new UserDTO() {Name = "Lucie", Surname= "Svoboda", NumberOfPoints = 1426, EmailAddress="Email14@gmail.com"},
            };

            var faculty1 = new FacultyDTO() { Name = Core.Enum.FacultyType.Unknown, Users = userArray1.ToList() };
            var faculty2 = new FacultyDTO() { Name = Core.Enum.FacultyType.Unknown2, Users = userArray2.ToList() };
            var faculty3 = new FacultyDTO() { Name = Core.Enum.FacultyType.Unknown3, Users = userArray3.ToList() };
            var faculty4 = new FacultyDTO() { Name = Core.Enum.FacultyType.Unknown4, Users = userArray4.ToList() };
            var faculty5 = new FacultyDTO() { Name = Core.Enum.FacultyType.Unknown5, Users = userArray5.ToList() };

            var university1 = new UniversityDTO() { Name = Core.Enum.UniversityType.Unknown, Faculties = new List<FacultyDTO>() { faculty1, faculty2 } };
            var university2 = new UniversityDTO() { Name = Core.Enum.UniversityType.Unknown2, Faculties = new List<FacultyDTO>() { faculty3, faculty4 } };

            return new UniversityDTO[] { university1, university2 };
        }

        public UniversityDTO GetUniversity(int id)
        {
            return dbModel.GetUniversityDTOById(id);
        }
        #endregion

        #region Events
        public event EventHandler? OnDatabaseUpdated;
        #endregion

    }
}
