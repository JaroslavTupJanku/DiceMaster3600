using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Model;
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
    public class HomeViewModel : ReactiveViewModel, IMenuControlViewModel
    {
        #region Fields
        private readonly IDataAccessManager? datamanager;
        private readonly GraphGenerator generator;
        private readonly IMessageService messageService;
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
        public HomeViewModel(IDataAccessManager manager, GraphGenerator generator, IMessageService messageService) : base(messageService)
        {
            datamanager = manager;
            this.generator = generator;
            this.messageService = messageService;

            Update().ConfigureAwait(false);

            ShowSuccessMessageCommand = new RelayCommand(() => snackbarMessageQueue.Enqueue(LoginSuccessMessage));
            datamanager.OnDatabaseUpdated += async (s, e) => await Update();
        }
        #endregion

        #region Methods

        public void Smazat()
        {
            messageService.NotifyImmediately($"User login was successful.");
        }

        public async Task Update()
        {
            try
            {
                var users = datamanager.GetAllUniversityDTOs().SelectMany(u => u.Faculties)
                                       .SelectMany(f => f.Users).Select(u => u.NumberOfPoints)
                                       .ToList();

                DataAccessManager X = (DataAccessManager)datamanager; //SMAYAT
                var topPlayers = X.TOPThreePlayersSmazat(); //await datamanager!.GetTopThreePlayersAsync();
                TopThreePlayerList = new ObservableCollection<RankedUserDTO>(topPlayers);

                var topUniversities = X.TOPThreeUNIVERSSmazat(); //await datamanager!.GetTopThreeUniversityAsync();
                TopThreeUniversityList = new ObservableCollection<UniversityRankingDTO>(topUniversities);

                UpdateHistogramSeries(users);

                TestPieGraphSeries = new ISeries[]
                {
                    new PieSeries<double> { Values = new double[] { 20 }, Name = "Fakulta A" },
                    new PieSeries<double> { Values = new double[] { 30 }, Name = "Fakulta B" },
                    new PieSeries<double> { Values = new double[] { 40 }, Name = "Fakulta C" },
                    new PieSeries<double> { Values = new double[] { 10 }, Name = "Fakulta D" },
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Update: " + ex.Message);
            }
        }

        public void UpdateHistogramSeries(List<int> values)
        {
            var data = generator.GenerateHistogram(values.ToArray(), 0, 375, 30);
            SeriesCollection = data.Series;
            XAxes = data.XAxes;
        }

        public override void RefreshCommand()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Events
        #endregion
    }
}
