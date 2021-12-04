using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class TripleExponentialMovingAverage : Indicator
    {
        // Single-, Double-, and Triple-Smoothed EMAs:
        // EMA1 = EMA of price
        // EMA2 = EMA of EMA1
        // EMA3 = EMA of EMA2
        // TEMA = (3 x EMA1) - (3 x EMA2) + (EMA3)

        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter("Periods", DefaultValue = 14)]
        public int Periods { get; set; }

        [Output("TEMA", LineColor = "DodgerBlue", Thickness = 2)]
        public IndicatorDataSeries TEMA { get; set; }

        private ExponentialMovingAverage EMA1, EMA2, EMA3;

        protected override void Initialize()
        {
            EMA1 = Indicators.ExponentialMovingAverage(Source, Periods);
            EMA2 = Indicators.ExponentialMovingAverage(EMA1.Result, Periods);
            EMA3 = Indicators.ExponentialMovingAverage(EMA2.Result, Periods);
        }

        public override void Calculate(int index)
        {
            TEMA[index] = (3 * EMA1.Result[index]) - (3 * EMA2.Result[index]) + EMA3.Result[index];
        }
    }
}
