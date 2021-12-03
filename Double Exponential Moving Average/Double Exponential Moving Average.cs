using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class DoubleExponentialMovingAverage : Indicator
    {
        // Single and Double-Smoothed EMAs:
        // EMA1 = EMA of price
        // EMA2 = EMA of EMA1
        // DEMA = (2 x EMA1) - EMA2

        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter("Periods", DefaultValue = 14)]
        public int Periods { get; set; }

        [Output("DEMA", LineColor = "DodgerBlue", Thickness = 2)]
        public IndicatorDataSeries DEMA { get; set; }

        private ExponentialMovingAverage EMA1, EMA2;

        protected override void Initialize()
        {
            EMA1 = Indicators.ExponentialMovingAverage(Source, Periods);
            EMA2 = Indicators.ExponentialMovingAverage(EMA1.Result, Periods);
        }

        public override void Calculate(int index)
        {
            DEMA[index] = (2 * EMA1.Result[index]) - EMA2.Result[index];
        }
    }
}
