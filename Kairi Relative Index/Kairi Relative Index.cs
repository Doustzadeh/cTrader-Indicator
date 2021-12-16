using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class KairiRelativeIndex : Indicator
    {
        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter("Periods", DefaultValue = 14)]
        public int Periods { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType MaType { get; set; }

        [Output("KRI", LineColor = "Red")]
        public IndicatorDataSeries KRI { get; set; }

        private MovingAverage MA;

        protected override void Initialize()
        {
            MA = Indicators.MovingAverage(Source, Periods, MaType);
        }

        public override void Calculate(int index)
        {
            KRI[index] = ((Source[index] - MA.Result[index]) / MA.Result[index]) * 100;
        }
    }
}
