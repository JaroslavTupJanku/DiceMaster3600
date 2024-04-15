using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Data.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiceMaster3600.Model
{
    public class DataAccessManager : IDataAccessManager
    {
        #region Fields
        private readonly RepositoryAdapter dbModel;
        private bool isProcessingData;
        #endregion

        #region Properties
        public bool IsProcessingData
        {
            get => isProcessingData;
            private set
            {
                if (isProcessingData != value)
                {
                    isProcessingData = value;
                    OnProcessingDataChanged?.Invoke(value);
                }
            }
        }
        #endregion

        #region Constructors
        public DataAccessManager(RepositoryAdapter dbModel) //AppSettings settings
        {
            this.dbModel = dbModel;
        }
        #endregion

        #region Methods
        private async Task<T> ExecuteDataOperationAsync<T>(Func<Task<T>> dataOperation)
        {
            try
            {
                IsProcessingData = true;
                return await dataOperation();
            }
            finally
            {
                IsProcessingData = false;
            }
        }

        private async Task ExecuteDataOperationAsync(Func<Task> dataOperation)
        {
            try
            {
                IsProcessingData = true;
                await dataOperation();
            }
            finally
            {
                IsProcessingData = false;
            }
        }

        public async Task<UserDTO[]> GetTopThreePlayersAsync()
        {
            return await ExecuteDataOperationAsync(() => dbModel.GetTopThree());
        }

        public async Task DeleteUniversityAsync(int id)
        {
            await ExecuteDataOperationAsync(async () =>
            {
                await dbModel.DeleteUniversityById(id);
                OnDatabaseUpdated?.Invoke(this, EventArgs.Empty);
            });
        }

        public async Task<UserDTO?> GetUserByEmailAsync(string email, string plainPassword)
        {
            return await ExecuteDataOperationAsync(() => dbModel.GetUserByEmailAsync(email, plainPassword));
        }

        public async Task RegisterUserAsync(UserDTO user, string plainPassword, UniversityType university, FacultyType faculty)
        {
            await ExecuteDataOperationAsync(() => dbModel.RegisterUser(user, plainPassword, university, faculty));
        }

        public async Task GetUniversityByID(int id)
        {
            await ExecuteDataOperationAsync(() => dbModel.GetUniversityDTOByIdAsync(id)); 
        }
        #endregion

        #region Events
        public event Action<bool>? OnProcessingDataChanged;
        public event EventHandler? OnDatabaseUpdated;
        #endregion

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

            var faculty1 = new FacultyDTO() { Name = FacultyType.None, Users = userArray1.ToList() };
            var faculty2 = new FacultyDTO() { Name = FacultyType.Unknown2, Users = userArray2.ToList() };
            var faculty3 = new FacultyDTO() { Name = Core.Enum.FacultyType.Unknown3, Users = userArray3.ToList() };
            var faculty4 = new FacultyDTO() { Name = Core.Enum.FacultyType.Unknown4, Users = userArray4.ToList() };
            var faculty5 = new FacultyDTO() { Name = Core.Enum.FacultyType.Unknown5, Users = userArray5.ToList() };

            var university1 = new UniversityDTO() { Name = Core.Enum.UniversityType.Unknown5, Faculties = new List<FacultyDTO>() { faculty1, faculty2 } };
            var university2 = new UniversityDTO() { Name = Core.Enum.UniversityType.Unknown2, Faculties = new List<FacultyDTO>() { faculty3, faculty4 } };

            return new UniversityDTO[] { university1, university2 };
        }

    }
}
