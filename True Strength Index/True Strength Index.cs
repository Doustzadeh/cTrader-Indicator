using System;
using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Levels(-25, 0, 25)]
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class TrueStrengthIndex : Indicator
    {
        // Double Smoothed PC
        // PC = Current Price minus Prior Price
        // First Smoothing = 25-period EMA of PC
        // Second Smoothing = 13-period EMA of 25-period EMA of PC

        // Double Smoothed Absolute PC
        // Absolute Price Change |PC| = Absolute Value of Current Price minus Prior Price
        // First Smoothing = 25-period EMA of |PC|
        // Second Smoothing = 13-period EMA of 25-period EMA of |PC|

        // TSI = 100 x (Double Smoothed PC / Double Smoothed Absolute PC)

        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter("First Smoothing", DefaultValue = 25, MinValue = 1)]
        public int FirstSmoothingPeriods { get; set; }

        [Parameter("Second Smoothing", DefaultValue = 13, MinValue = 1)]
        public int SecondSmoothingPeriods { get; set; }

        [Parameter("Signal Periods", DefaultValue = 7, MinValue = 1)]
        public int SignalPeriods { get; set; }

        [Output("TSI", LineColor = "DodgerBlue")]
        public IndicatorDataSeries TSI { get; set; }

        [Output("Signal", LineColor = "Red")]
        public IndicatorDataSeries Signal { get; set; }

        private IndicatorDataSeries PC, APC;
        private ExponentialMovingAverage FirstSmoothingPC, SecondSmoothingPC, FirstSmoothingAPC, SecondSmoothingAPC;
        private ExponentialMovingAverage SignalEMA;

        protected override void Initialize()
        {
            PC = CreateDataSeries();
            FirstSmoothingPC = Indicators.ExponentialMovingAverage(PC, FirstSmoothingPeriods);
            SecondSmoothingPC = Indicators.ExponentialMovingAverage(FirstSmoothingPC.Result, SecondSmoothingPeriods);

            APC = CreateDataSeries();
            FirstSmoothingAPC = Indicators.ExponentialMovingAverage(APC, FirstSmoothingPeriods);
            SecondSmoothingAPC = Indicators.ExponentialMovingAverage(FirstSmoothingAPC.Result, SecondSmoothingPeriods);

            SignalEMA = Indicators.ExponentialMovingAverage(TSI, SignalPeriods);
        }

        public override void Calculate(int index)
        {
            PC[index] = Source[index] - Source[index - 1];
            APC[index] = Math.Abs(PC[index]);
            TSI[index] = 100 * (SecondSmoothingPC.Result[index] / SecondSmoothingAPC.Result[index]);

            Signal[index] = SignalEMA.Result[index];
        }
    }
}
