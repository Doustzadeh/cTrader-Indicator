using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Levels(0, 0.2, 0.5, 0.8, 1)]
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    class StochRSI : Indicator
    {
        // StochRSI = (RSI - Lowest Low RSI) / (Highest High RSI - Lowest Low RSI)

        // 14-day StochRSI equals 0 when RSI is at its lowest point for 14 days
        // 14-day StochRSI equals 1 when RSI is at its highest point for 14 days
        // 14-day StochRSI equals 0.5 when RSI is in the middle of its 14-day high-low range
        // 14-day StochRSI equals 0.2 when RSI is near the low of its 14-day high-low range
        // 14-day StochRSI equals 0.80 when RSI is near the high of its 14-day high-low range

        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter("Periods", DefaultValue = 14)]
        public int Periods { get; set; }

        [Output("Stoch RSI", LineColor = "Red")]
        public IndicatorDataSeries StochRsi { get; set; }

        private RelativeStrengthIndex RSI;

        protected override void Initialize()
        {
            RSI = Indicators.RelativeStrengthIndex(Source, Periods);
        }

        public override void Calculate(int index)
        {
            StochRsi[index] = (RSI.Result[index] - RSI.Result.Minimum(Periods)) / (RSI.Result.Maximum(Periods) - RSI.Result.Minimum(Periods));
        }
    }
}
