using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DiceMaster3600.Model.Services
{
    public class HistogramData
    {
        public ISeries[] Series { get; }
        public Axis[] XAxes { get; }

        public HistogramData(ISeries[] series, Axis[] xAxes)
        {
            Series = series;
            XAxes = xAxes;
        }
    }

    public class GraphGenerator
    {
        public HistogramData GenerateHistogram(int[] values, int minValue, int maxValue, int intervalSize)
        {
            var histogramValues = new ObservableCollection<double>();
            for (int i = minValue; i < maxValue; i += intervalSize)
            {
                var count = values.Count(v => v >= i && v < i + intervalSize);
                histogramValues.Add(count);
            }

            var SeriesCollection = new ISeries[]
            {
                new ColumnSeries<double> { Values = histogramValues }
            };

            var labels = new List<string>();
            for (int i = minValue; i < maxValue; i += intervalSize)
            {
                labels.Add($"{i} - {i + intervalSize}");
            }

            var XAxes = new Axis[] { GetAxis(labels.ToArray()) };

            return new HistogramData(SeriesCollection, XAxes);
        }

        private Axis GetAxis(string[] labels)
        {
            return new Axis
            {
                Labels = labels,
                LabelsRotation = 0,
                SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                SeparatorsAtCenter = false,
                TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                TicksAtCenter = true,
                ForceStepToMin = true,
                MinStep = 1
            };
        }
    }
}
