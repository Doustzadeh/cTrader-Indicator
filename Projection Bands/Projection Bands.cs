using System;
using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class ProjectionBands : Indicator
    {
        [Parameter("Periods", DefaultValue = 14)]
        public int Periods { get; set; }

        [Output("HiBand", LineColor = "DodgerBlue")]
        public IndicatorDataSeries HiBand { get; set; }

        [Output("LowBand", LineColor = "Red")]
        public IndicatorDataSeries LowBand { get; set; }

        private LinearRegressionSlope LrsHigh, LrsLow;

        protected override void Initialize()
        {
            LrsHigh = Indicators.LinearRegressionSlope(Bars.HighPrices, Periods);
            LrsLow = Indicators.LinearRegressionSlope(Bars.LowPrices, Periods);
        }

        public override void Calculate(int index)
        {
            HiBand[index] = Bars.HighPrices[index];
            LowBand[index] = Bars.LowPrices[index];

            for (int i = 1; i < Periods; i++)
            {
                HiBand[index] = Math.Max(HiBand[index], Bars.HighPrices.Last(i) + i * LrsHigh.Result[index]);
                LowBand[index] = Math.Min(LowBand[index], Bars.LowPrices.Last(i) + i * LrsLow.Result[index]);
            }
        }
    }
}
