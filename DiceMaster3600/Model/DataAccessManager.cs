using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiceMaster3600.Model
{
    public class DataAccessManager : IDataAccessManager
    {
        #region Fields
        private readonly RepositoryFasade dbModel;
        private bool isworking;

        List<UniversityDTO> smazat = new List<UniversityDTO>(); 
        #endregion

        #region Properties
        public bool IsWorking
        {
            get => isworking;
            private set
            {
                if (isworking != value)
                {
                    isworking = value;
                    OnProcessingStatusChanged?.Invoke(value);
                }
            }
        }
        #endregion

        #region Constructors
        public DataAccessManager(RepositoryFasade dbModel)
        {
            this.dbModel = dbModel;
        }
        #endregion

        #region Methods
        private async Task<T> ExecuteDataOperationAsync<T>(Func<Task<T>> dataOperation)
        {
            try
            {
                IsWorking = true;
                return await dataOperation();
            }
            finally
            {
                IsWorking = false;
            }
        }

        private async Task ExecuteDataOperationAsync(Func<Task> dataOperation)
        {
            try
            {
                IsWorking = true;
                await dataOperation();
            }
            finally
            {
                IsWorking = false;
            }
        }

        public async Task DeleteUniversityAsync(int id)
        {
            await ExecuteDataOperationAsync(async () =>
            {
                await dbModel.DeleteUniversityById(id);
                OnDatabaseUpdated?.Invoke(this, EventArgs.Empty);
            });
        }

        public async Task<RankedUserDTO[]> GetTopThreePlayersAsync() 
            => await ExecuteDataOperationAsync(() 
            => dbModel.GetTopThree());

        public async Task<UniversityRankingDTO[]> GetTopThreeUniversityAsync()
            => await ExecuteDataOperationAsync(() 
            => dbModel.GetTopThreeUniversity());

        public async Task<UserDTO?> GetUserByEmailAsync(string email, string plainPassword) 
            => await ExecuteDataOperationAsync(() 
            => dbModel.GetUserByEmailAsync(email, plainPassword));

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
        public event Action<bool>? OnProcessingStatusChanged;
        public event EventHandler? OnDatabaseUpdated;
        #endregion


        public RankedUserDTO[] TOPThreePlayersSmazat()
        {
            RankedUserDTO u = new RankedUserDTO(smazat[1].Faculties[1].Users[1], 1);
            RankedUserDTO V = new RankedUserDTO(smazat[0].Faculties[0].Users[1], 2);
            RankedUserDTO W = new RankedUserDTO(smazat[0].Faculties[2].Users[0], 3);

            return new RankedUserDTO[] {u, V, W };
        }

        public UniversityRankingDTO[] TOPThreeUNIVERSSmazat()
        {
            UniversityRankingDTO u = new UniversityRankingDTO()
            {
                UniversityName = UniversityType.Unknown2,
                Position = 1,
                StudentCount = 6,
                AveragePoints=135             
            };

            UniversityRankingDTO V = new UniversityRankingDTO()
            {
                UniversityName = UniversityType.Unknown2,
                Position = 2,
                StudentCount = 5,
                AveragePoints = 112
            };

            UniversityRankingDTO B = new UniversityRankingDTO()
            {
                UniversityName = UniversityType.Unknown2,
                Position = 3,
                StudentCount = 2,
                AveragePoints = 101
            };

            return new UniversityRankingDTO[] { u, V, B };
        }

        public UniversityDTO[] GetAllUniversityDTOs()
        {
            var userArray1 = new UserDTO[] {
                new UserDTO() {Name = "Hana", Surname= "Černý", NumberOfPoints = 129, Gender=Gender.None},
                new UserDTO() {Name = "Pavel", Surname= "Horák", NumberOfPoints = 208, Gender=Gender.Male},
                new UserDTO() {Name = "Pavel", Surname= "Dvořák", NumberOfPoints = 38, Gender=Gender.Male},
            };

           var userArray2 = new UserDTO[] {
                new UserDTO() {Name = "Pavel", Surname= "Kučera", NumberOfPoints = 91, Gender=Gender.Male},
                new UserDTO() {Name = "Jan", Surname= "Kučera", NumberOfPoints = 14, Gender=Gender.Male},
                new UserDTO() {Name = "Jan", Surname= "Kučera", NumberOfPoints = 86, Gender=Gender.Male},
            };

            var userArray3 = new UserDTO[] {
                new UserDTO() {Name = "Jan", Surname= "Novotný", NumberOfPoints = 101, Gender=Gender.Male},
                new UserDTO() {Name = "Marie", Surname= "Dvořák", NumberOfPoints = 20, Gender=Gender.None},
                new UserDTO() {Name = "Anna", Surname= "Krhutová", NumberOfPoints = 109, Gender=Gender.Female},
            };

            var userArray4 = new UserDTO[] {
                new UserDTO() {Name = "Martin", Surname= "Svoboda", NumberOfPoints = 229, Gender=Gender.Male},
                new UserDTO() {Name = "Petr", Surname= "Novotný", NumberOfPoints = 278, Gender=Gender.Male},
                new UserDTO() {Name = "Marie", Surname= "Novotný", NumberOfPoints = 102, Gender=Gender.Female},
            };

            var userArray5 = new UserDTO[] {
                new UserDTO() {Name = "Lucie", Surname= "Procházka", NumberOfPoints = 197, Gender=Gender.Female},
                new UserDTO() {Name = "Marie", Surname= "Novotný", NumberOfPoints = 155, Gender=Gender.Male},
                new UserDTO() {Name = "Lucie", Surname= "Svoboda", NumberOfPoints = 111, Gender=Gender.Male},
            };

            var faculty1 = new FacultyDTO() { Name = FacultyType.None, Users = userArray1.ToList() };
            var faculty2 = new FacultyDTO() { Name = FacultyType.Unknown2, Users = userArray2.ToList() };
            var faculty3 = new FacultyDTO() { Name = Core.Enum.FacultyType.Unknown3, Users = userArray3.ToList() };
            var faculty4 = new FacultyDTO() { Name = Core.Enum.FacultyType.Unknown4, Users = userArray4.ToList() };
            var faculty5 = new FacultyDTO() { Name = Core.Enum.FacultyType.Unknown5, Users = userArray5.ToList() };

            var university1 = new UniversityDTO() { Name = Core.Enum.UniversityType.Unknown5, Faculties = new List<FacultyDTO>() { faculty1, faculty2, faculty5 } };
            var university2 = new UniversityDTO() { Name = Core.Enum.UniversityType.Unknown2, Faculties = new List<FacultyDTO>() { faculty3, faculty4 } };

            smazat = new UniversityDTO[] { university1, university2 }.ToList();
            return new UniversityDTO[] { university1, university2 };
        }

    }
}
