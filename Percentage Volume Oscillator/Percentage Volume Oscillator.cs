using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class PercentageVolumeOscillator : Indicator
    {
        // Percentage Volume Oscillator (PVO): ((12-day EMA of Volume - 26-day EMA of Volume) / 26-day EMA of Volume) x 100
        // Signal Line: 9-day EMA of PVO
        // PVO Histogram: PVO - Signal Line

        [Parameter("Long Cycle", DefaultValue = 26)]
        public int LongCycle { get; set; }

        [Parameter("Short Cycle", DefaultValue = 12)]
        public int ShortCycle { get; set; }

        [Parameter("Signal Periods", DefaultValue = 9)]
        public int SignalPeriods { get; set; }

        [Output("PVO", LineColor = "DodgerBlue")]
        public IndicatorDataSeries PVO { get; set; }

        [Output("Signal", LineColor = "Red", LineStyle = LineStyle.Lines)]
        public IndicatorDataSeries Signal { get; set; }

        [Output("Histogram", LineColor = "White", PlotType = PlotType.Histogram)]
        public IndicatorDataSeries Histogram { get; set; }

        private ExponentialMovingAverage LongEMA, ShortEMA, SignalEMA;

        protected override void Initialize()
        {
            LongEMA = Indicators.ExponentialMovingAverage(Bars.TickVolumes, LongCycle);
            ShortEMA = Indicators.ExponentialMovingAverage(Bars.TickVolumes, ShortCycle);
            SignalEMA = Indicators.ExponentialMovingAverage(PVO, SignalPeriods);
        }

        public override void Calculate(int index)
        {
            PVO[index] = ((ShortEMA.Result[index] - LongEMA.Result[index]) / LongEMA.Result[index]) * 100;
            Signal[index] = SignalEMA.Result[index];
            Histogram[index] = PVO[index] - Signal[index];
        }
    }
}
