using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class WaddahAttarExplosion : Indicator
    {
        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter("Periods", Group = "Bollinger Bands", DefaultValue = 20)]
        public int BollingerPeriods { get; set; }

        [Parameter("Standard Dev", Group = "Bollinger Bands", DefaultValue = 2, MinValue = 0)]
        public double StDeviation { get; set; }

        [Parameter("MA Type", Group = "Bollinger Bands", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType MaType { get; set; }

        [Parameter("Long Cycle", Group = "MACD", DefaultValue = 40)]
        public int LongCycle { get; set; }

        [Parameter("Short Cycle", Group = "MACD", DefaultValue = 20)]
        public int ShortCycle { get; set; }

        [Parameter("Signal Periods", Group = "MACD", DefaultValue = 9)]
        public int SignalPeriods { get; set; }

        [Parameter("Sensetive", DefaultValue = 150)]
        public int Sensetive { get; set; }

        [Parameter("Dead Zone Pip", DefaultValue = 30)]
        public int DeadZonePip { get; set; }

        [Output("Trend", LineColor = "Lime", PlotType = PlotType.Histogram, Thickness = 4)]
        public IndicatorDataSeries Trend { get; set; }

        [Output("iTrend", LineColor = "Red", PlotType = PlotType.Histogram, Thickness = 4)]
        public IndicatorDataSeries iTrend { get; set; }

        [Output("Explosion", LineColor = "Yellow", Thickness = 2)]
        public IndicatorDataSeries Explosion { get; set; }

        [Output("Dead", LineColor = "DodgerBlue", Thickness = 2)]
        public IndicatorDataSeries Dead { get; set; }

        private MacdCrossOver iMACD;
        private BollingerBands iBands;

        private double TrendDir;

        protected override void Initialize()
        {
            iMACD = Indicators.MacdCrossOver(Source, LongCycle, ShortCycle, SignalPeriods);
            iBands = Indicators.BollingerBands(Source, BollingerPeriods, StDeviation, MaType);
        }

        public override void Calculate(int index)
        {
            TrendDir = (iMACD.MACD[index] - iMACD.MACD[index - 1]) * Sensetive;

            if (TrendDir >= 0)
            {
                Trend[index] = TrendDir;
            }

            if (TrendDir < 0)
            {
                iTrend[index] = -1 * TrendDir;
            }

            Explosion[index] = iBands.Top[index] - iBands.Bottom[index];
            Dead[index] = Symbol.TickSize * DeadZonePip;
        }

    }
}
