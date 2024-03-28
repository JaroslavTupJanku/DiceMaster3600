using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace DiceMaster3600.ViewModel
{
    public class HomeViewModel : ObservableObject
    {
        #region Fields
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
        #endregion

        #region Constructors
        public HomeViewModel()
        {

        }
        #endregion

        #region Methods
        #endregion

        #region Events
        #endregion

    }
}
