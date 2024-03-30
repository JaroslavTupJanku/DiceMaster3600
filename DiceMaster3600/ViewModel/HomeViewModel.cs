using CommunityToolkit.Mvvm.ComponentModel;
using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Model;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using System.Linq;

namespace DiceMaster3600.ViewModel
{
    public class HomeViewModel : ObservableObject, IMenuControlViewModel
    {
        #region Fields
        private readonly IDataAccessManager? datamanager;
        private ISeries[]? testPieGraphSeries;
        #endregion

        #region Properties
        public ISeries[] Series { get; set; } = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                Fill = null
            }
        };

        public ISeries[]? TestPieGraphSeries
        {
            get => testPieGraphSeries;
            set => SetProperty(ref testPieGraphSeries, value);
        }

        public ObservableCollection<UserWithPosition> TopThreeList { get; set; } = new();
        public MenuControlType ControlType => MenuControlType.DashBoard;
        #endregion

        #region Constructors
        public HomeViewModel(IDataAccessManager manager)
        {
            datamanager = manager;
            Update();

            datamanager.OnDatabaseUpdated += (s, e) => Update();
        }
        #endregion

        #region Methods
        public void Update()
        {
            TopThreeList = new(datamanager!.GetTopThreePlayers().Select((user, index) => new UserWithPosition 
            {
                User = user,
                Position = index + 1
            }));

            TestNevim();
        }

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
        #endregion

        #region Events
        #endregion

    }
}
