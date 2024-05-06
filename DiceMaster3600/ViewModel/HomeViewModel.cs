using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Model.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiceMaster3600.ViewModel
{
    public class HomeViewModel : ObservableObject, IMenuControlViewModel
    {
        #region Fields
        private readonly IDataAccessManager? datamanager;
        private readonly GraphGenerator generator;
        private readonly SnackbarMessageQueue snackbarMessageQueue = new(TimeSpan.FromSeconds(3));

        private string loginSuccessMessage = "You have successfully logged in!";
        private ISeries[]? testPieGraphSeries;
        private ISeries[]? testGaussSeries;
        #endregion

        #region Properties
        public ISeries[]? TesteGaussSeries
        {
            get => testGaussSeries;
            set => SetProperty(ref testGaussSeries, value);
        }

        public ISeries[]? TestPieGraphSeries
        {
            get => testPieGraphSeries;
            set => SetProperty(ref testPieGraphSeries, value);
        }

        public string LoginSuccessMessage
        {
            get => loginSuccessMessage;
            set => SetProperty(ref loginSuccessMessage, value);
        }

        public ISeries[] SeriesCollection { get; set; }
        public Axis[] XAxes { get; set; }

        public MenuControlType ControlType => MenuControlType.DashBoard;
        public SnackbarMessageQueue SnackbarMessageQueue => snackbarMessageQueue;
        public ObservableCollection<RankedUserDTO> TopThreePlayerList { get; set; }
        public ObservableCollection<UniversityRankingDTO> TopThreeUniversityList { get; set; }

        public ICommand ShowSuccessMessageCommand { get; }
        #endregion

        #region Constructors

        public HomeViewModel(IDataAccessManager manager, GraphGenerator generator)
        {
            datamanager = manager;
            this.generator = generator;

            TestNevim();
            Update().ConfigureAwait(false);

            ShowSuccessMessageCommand = new RelayCommand(() => snackbarMessageQueue.Enqueue(LoginSuccessMessage));
            datamanager.OnDatabaseUpdated += async (s, e) => await Update();
        }
        #endregion

        #region Methods
        public async Task Update()
        {
            try
            {
                //var topPlayers = await datamanager!.GetTopThreePlayersAsync();
                //var topUniversities = await datamanager!.GetTopThreeUniversityAsync();

                //TopThreePlayerList = new ObservableCollection<RankedUserDTO>(topPlayers);
                //TopThreeUniversityList = new ObservableCollection<UniversityRankingDTO>(topUniversities);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Update: " + ex.Message);
            }
        }
        #endregion

        #region Events
        #endregion


        public void TestNevim()
        {
            GetAllUniversityDTOs();
        }

        public void GetAllUniversityDTOs()
        {
            var userArray1 = new UserDTO[] {
                new UserDTO() {Name = "Hana", Surname= "Černý", NumberOfPoints = 129, Gender = Gender.None},
                new UserDTO() {Name = "Pavel", Surname= "Horák", NumberOfPoints = 208, Gender = Gender.Male},
                new UserDTO() {Name = "Pavel", Surname= "Dvořák", NumberOfPoints = 38, Gender = Gender.Male},
            };

            var userArray2 = new UserDTO[] {
                new UserDTO() {Name = "Pavel", Surname= "Kučera", NumberOfPoints = 91, Gender = Gender.Male},
                new UserDTO() {Name = "Jan", Surname= "Kučera", NumberOfPoints = 14, Gender = Gender.Male},
                new UserDTO() {Name = "Jan", Surname= "Kučera", NumberOfPoints = 86, Gender = Gender.Male},
            };

            var userArray3 = new UserDTO[] {
                new UserDTO() {Name = "Jan", Surname= "Novotný", NumberOfPoints = 101, Gender = Gender.Male},
                new UserDTO() {Name = "Marie", Surname= "Dvořák", NumberOfPoints = 20, Gender = Gender.Female},
                new UserDTO() {Name = "Anna", Surname= "Krhutová", NumberOfPoints = 109, Gender = Gender.Female},
            };

            var userArray4 = new UserDTO[] {
                new UserDTO() {Name = "Martin", Surname= "Svoboda", NumberOfPoints = 195, Gender = Gender.Male},
                new UserDTO() {Name = "Petr", Surname= "Novotný", NumberOfPoints = 278, Gender = Gender.Male},
                new UserDTO() {Name = "Marie", Surname= "Novotný", NumberOfPoints = 102, Gender = Gender.Female},
            };

            var userArray5 = new UserDTO[] {
                new UserDTO() {Name = "Lucie", Surname= "Procházka", NumberOfPoints = 197, Gender = Gender.Female},
                new UserDTO() {Name = "Marie", Surname= "Novotný", NumberOfPoints = 155, Gender = Gender.Female},
                new UserDTO() {Name = "Lucie", Surname= "Svoboda", NumberOfPoints = 111, Gender = Gender.Female},
            };


            //University procenta
            TestPieGraphSeries = new ISeries[]
            {
                new PieSeries<double> { Values = new double[] { 20 }, Name = "Fakulta A" },
                new PieSeries<double> { Values = new double[] { 30 }, Name = "Fakulta B" },
                new PieSeries<double> { Values = new double[] { 40 }, Name = "Fakulta C" },
                new PieSeries<double> { Values = new double[] { 10 }, Name = "Fakulta D" },
            };

            //Propabily
            UpdateHistogramSeries(userArray1, userArray2, userArray3, userArray4, userArray5);

            //Top Three Player
            var first = new RankedUserDTO(userArray4[1], 1);
            var second = new RankedUserDTO(userArray1[1], 2);
            var third = new RankedUserDTO(userArray5[0], 3);
            var topPlayers = new List<RankedUserDTO>() { first, second, third };
            TopThreePlayerList = new ObservableCollection<RankedUserDTO>(topPlayers);

            //Top Three university
            var firstUniversity = new UniversityRankingDTO() { AveragePoints = 255, StudentCount = 10, UniversityName = UniversityType.Unknown2, Position = 1 };
            var secondUniversity = new UniversityRankingDTO() { AveragePoints = 205, StudentCount = 8, UniversityName = UniversityType.Unknown2, Position = 2 };
            var thirdUniversity = new UniversityRankingDTO() { AveragePoints = 194, StudentCount = 5, UniversityName = UniversityType.Unknown2, Position = 3 };
            var topUniversities = new List<UniversityRankingDTO>() { firstUniversity, secondUniversity, thirdUniversity };
            TopThreeUniversityList = new ObservableCollection<UniversityRankingDTO>(topUniversities);
        }

        public void UpdateHistogramSeries(UserDTO[] userArray1, UserDTO[] userArray2, UserDTO[] userArray3, UserDTO[] userArray4, UserDTO[] userArray5)
        {
            var values = new List<int>();
            values.AddRange(userArray1.Select(x => x.NumberOfPoints));
            values.AddRange(userArray2.Select(x => x.NumberOfPoints));
            values.AddRange(userArray3.Select(x => x.NumberOfPoints));
            values.AddRange(userArray4.Select(x => x.NumberOfPoints));
            values.AddRange(userArray5.Select(x => x.NumberOfPoints));

            var data = generator.GenerateHistogram(values.ToArray(), 0, 375, 30);
            SeriesCollection = data.Series;
            XAxes = data.XAxes;

        }
    }
}
