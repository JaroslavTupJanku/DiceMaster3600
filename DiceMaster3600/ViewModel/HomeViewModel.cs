using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Model;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiceMaster3600.ViewModel
{
    public class HomeViewModel : ObservableObject, IMenuControlViewModel
    {
        #region Fields
        private readonly IDataAccessManager? datamanager;
        private readonly SnackbarMessageQueue snackbarMessageQueue = new(TimeSpan.FromSeconds(3));
        private string loginSuccessMessage = "You have successfully logged in!";
        private ISeries[]? testPieGraphSeries;
        #endregion

        #region Properties
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

        public MenuControlType ControlType => MenuControlType.DashBoard;
        public SnackbarMessageQueue SnackbarMessageQueue => snackbarMessageQueue;
        public ICommand ShowSuccessMessageCommand { get; }
        public ObservableCollection<UserWithPosition> TopThreeList { get; set; } = new();
        #endregion

        #region Constructors

        public HomeViewModel(IDataAccessManager manager)
        {
            datamanager = manager;
            TestNevim();
            Update().ConfigureAwait(false);

            ShowSuccessMessageCommand = new RelayCommand(() => snackbarMessageQueue.Enqueue(LoginSuccessMessage));
            datamanager.OnDatabaseUpdated += async (s, e) => await Update();
        }
        #endregion

        #region Methods
        public async Task Update()
        {
            var topThreePlayers = await datamanager!.GetTopThreePlayersAsync();
            TopThreeList = new(topThreePlayers.Select((user, index) => new UserWithPosition(user, index + 1)));
        }
        #endregion

        #region Events
        #endregion


        public void TestNevim()
        {
            TestPieGraphSeries = new ISeries[]
            {
                new PieSeries<double> { Values = new double[] { 100 }, Name = "Fakulta A" },
                new PieSeries<double> { Values = new double[] { 200 }, Name = "Fakulta B" },
                new PieSeries<double> { Values = new double[] { 150 }, Name = "Fakulta C" },
                new PieSeries<double> { Values = new double[] { 250 }, Name = "Fakulta D" },
            };
        }



        public ISeries[] Series { get; set; } = new ISeries[]
{
            new LineSeries<double>
            {
                Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                Fill = null
            }
};


    }
}
