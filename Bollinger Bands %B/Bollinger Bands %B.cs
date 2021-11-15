using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Levels(0, 0.5, 1)]
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class BollingerBandsPercentB : Indicator
    {
        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter("Periods", Group = "Bollinger Bands", DefaultValue = 20)]
        public int Periods { get; set; }

        [Parameter("Standard Dev", Group = "Bollinger Bands", DefaultValue = 2)]
        public double StDeviation { get; set; }

        [Parameter("MA Type", Group = "Bollinger Bands", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType MaType { get; set; }

        [Output("Main", LineColor = "Red")]
        public IndicatorDataSeries Result { get; set; }

        private BollingerBands BBands;

        protected override void Initialize()
        {
            BBands = Indicators.BollingerBands(Source, Periods, StDeviation, MaType);
        }

        public override void Calculate(int index)
        {
            Result[index] = (Source[index] - BBands.Bottom[index]) / (BBands.Top[index] - BBands.Bottom[index]);
        }

    }
}
