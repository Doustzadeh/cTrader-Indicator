using System;
using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Levels(20, 80)]
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class ProjectionOscillator : Indicator
    {
        [Parameter("Periods", Group = "PO", DefaultValue = 14)]
        public int Periods { get; set; }

        [Parameter("MA Periods", Group = "Signal", DefaultValue = 5)]
        public int MaPeriods { get; set; }

        [Parameter("MA Type", Group = "Signal", DefaultValue = MovingAverageType.Exponential)]
        public MovingAverageType MaType { get; set; }

        [Output("PO", LineColor = "DodgerBlue")]
        public IndicatorDataSeries PO { get; set; }

        [Output("Signal", LineColor = "Red")]
        public IndicatorDataSeries Signal { get; set; }

        private LinearRegressionSlope LrsHigh, LrsLow;
        private MovingAverage MaSignal;

        protected override void Initialize()
        {
            LrsHigh = Indicators.LinearRegressionSlope(Bars.HighPrices, Periods);
            LrsLow = Indicators.LinearRegressionSlope(Bars.LowPrices, Periods);
            MaSignal = Indicators.MovingAverage(PO, MaPeriods, MaType);
        }

        public override void Calculate(int index)
        {
            double HiBand = Bars.HighPrices[index];
            double LowBand = Bars.LowPrices[index];

            for (int i = 1; i < Periods; i++)
            {
                HiBand = Math.Max(HiBand, Bars.HighPrices.Last(i) + i * LrsHigh.Result[index]);
                LowBand = Math.Min(LowBand, Bars.LowPrices.Last(i) + i * LrsLow.Result[index]);
            }

            PO[index] = 100 * (Bars.ClosePrices[index] - LowBand) / (HiBand - LowBand);
            Signal[index] = MaSignal.Result[index];
        }
    }
}
