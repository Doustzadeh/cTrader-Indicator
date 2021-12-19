using System;
using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class ProjectionBandwidth : Indicator
    {
        [Parameter("Periods", DefaultValue = 14)]
        public int Periods { get; set; }

        [Output("BW", LineColor = "Red")]
        public IndicatorDataSeries BW { get; set; }

        private LinearRegressionSlope LrsHigh, LrsLow;

        protected override void Initialize()
        {
            LrsHigh = Indicators.LinearRegressionSlope(Bars.HighPrices, Periods);
            LrsLow = Indicators.LinearRegressionSlope(Bars.LowPrices, Periods);
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

            BW[index] = 200 * (HiBand - LowBand) / (HiBand + LowBand);
        }
    }
}
