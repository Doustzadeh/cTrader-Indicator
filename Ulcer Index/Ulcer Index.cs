using System;
using cAlgo.API;

namespace cAlgo
{
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class UlcerIndex : Indicator
    {
        // Percent-Drawdown = ((Close - 14-period Max Close) / 14-period Max Close) x 100
        // Squared Average = (14-period Sum of Percent-Drawdown Squared) / 14 
        // Ulcer Index = Square Root of Squared Average

        [Parameter("Periods", DefaultValue = 14)]
        public int Periods { get; set; }

        [Output("Ulcer Index", LineColor = "Red")]
        public IndicatorDataSeries Result { get; set; }

        private IndicatorDataSeries PercentDrawdown, PercentDrawdownSquared;

        protected override void Initialize()
        {
            PercentDrawdown = CreateDataSeries();
            PercentDrawdownSquared = CreateDataSeries();
        }

        public override void Calculate(int index)
        {
            PercentDrawdown[index] = ((Bars.ClosePrices[index] - Bars.ClosePrices.Maximum(Periods)) / Bars.ClosePrices.Maximum(Periods)) * 100;
            PercentDrawdownSquared[index] = Math.Pow(PercentDrawdown[index], 2);
            double SquaredAverage = PercentDrawdownSquared.Sum(Periods) / Periods;
            Result[index] = Math.Sqrt(SquaredAverage);
        }
    }
}
