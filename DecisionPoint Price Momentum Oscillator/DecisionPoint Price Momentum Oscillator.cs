using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class DecisionPointPriceMomentumOscillator : Indicator
    {
        // Smoothing Multiplier = (2 / Time period)
        // Custom Smoothing Function = {Close - Smoothing Function(previous day)} * Smoothing Multiplier + Smoothing Function(previous day)
        // PMO Line = 20-period Custom Smoothing of (10 * 35-period Custom Smoothing of (((Today's Price/Yesterday's Price) * 100) - 100))
        // PMO Signal Line = 10-period EMA of the PMO Line

        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter("Long Cycle", DefaultValue = 35)]
        public int LongCycle { get; set; }

        [Parameter("Short Cycle", DefaultValue = 20)]
        public int ShortCycle { get; set; }

        [Parameter("Signal Periods", DefaultValue = 10)]
        public int SignalPeriods { get; set; }

        [Output("PMO", LineColor = "DodgerBlue")]
        public IndicatorDataSeries PMO { get; set; }

        [Output("Signal", LineColor = "Red", LineStyle = LineStyle.Lines)]
        public IndicatorDataSeries Signal { get; set; }

        [Output("Histogram", LineColor = "White", PlotType = PlotType.Histogram)]
        public IndicatorDataSeries Histogram { get; set; }

        private IndicatorDataSeries CustomSmoothingLong, CustomSmoothingShort;
        private ExponentialMovingAverage SignalEMA;

        protected override void Initialize()
        {
            CustomSmoothingLong = CreateDataSeries();
            CustomSmoothingShort = CreateDataSeries();

            SignalEMA = Indicators.ExponentialMovingAverage(PMO, SignalPeriods);
        }

        public override void Calculate(int index)
        {
            if (index < 1)
            {
                CustomSmoothingLong[index] = 0;
                CustomSmoothingShort[index] = 0;
                return;
            }

            double ROC = ((Source[index] / Source[index - 1]) * 100) - 100;
            CustomSmoothingLong[index] = ((ROC - CustomSmoothingLong[index - 1]) * (2.0 / LongCycle)) + CustomSmoothingLong[index - 1];
            CustomSmoothingShort[index] = (((10 * CustomSmoothingLong[index]) - CustomSmoothingShort[index - 1]) * (2.0 / ShortCycle)) + CustomSmoothingShort[index - 1];

            PMO[index] = CustomSmoothingShort[index];
            Signal[index] = SignalEMA.Result[index];
            Histogram[index] = PMO[index] - Signal[index];
        }
    }
}
