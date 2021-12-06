using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class PercentagePriceOscillator : Indicator
    {
        // Percentage Price Oscillator (PPO): {(12-day EMA - 26-day EMA)/26-day EMA} x 100
        // Signal Line: 9-day EMA of PPO
        // PPO Histogram: PPO - Signal Line

        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter("Long Cycle", DefaultValue = 26)]
        public int LongCycle { get; set; }

        [Parameter("Short Cycle", DefaultValue = 12)]
        public int ShortCycle { get; set; }

        [Parameter("Signal Periods", DefaultValue = 9)]
        public int SignalPeriods { get; set; }

        [Output("PPO", LineColor = "DodgerBlue")]
        public IndicatorDataSeries PPO { get; set; }

        [Output("Signal", LineColor = "Red", LineStyle = LineStyle.Lines)]
        public IndicatorDataSeries Signal { get; set; }

        [Output("Histogram", LineColor = "White", PlotType = PlotType.Histogram)]
        public IndicatorDataSeries Histogram { get; set; }

        private ExponentialMovingAverage LongEMA, ShortEMA, SignalEMA;

        protected override void Initialize()
        {
            LongEMA = Indicators.ExponentialMovingAverage(Source, LongCycle);
            ShortEMA = Indicators.ExponentialMovingAverage(Source, ShortCycle);
            SignalEMA = Indicators.ExponentialMovingAverage(PPO, SignalPeriods);
        }

        public override void Calculate(int index)
        {
            PPO[index] = ((ShortEMA.Result[index] - LongEMA.Result[index]) / LongEMA.Result[index]) * 100;
            Signal[index] = SignalEMA.Result[index];
            Histogram[index] = PPO[index] - Signal[index];
        }
    }
}
