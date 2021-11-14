using System;
using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class SqueezeBreak : Indicator
    {
        [Parameter("Periods", Group = "Bollinger Bands", DefaultValue = 20)]
        public int BollingerPeriod { get; set; }

        [Parameter("Standard Dev", Group = "Bollinger Bands", DefaultValue = 2, MinValue = 0)]
        public double StDeviation { get; set; }

        [Parameter("MA Type", Group = "Bollinger Bands", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType BollingerMaType { get; set; }

        [Parameter("MA Period", Group = "Keltner Channels", DefaultValue = 20)]
        public int KeltnerPeriod { get; set; }

        [Parameter("MA Type", Group = "Keltner Channels", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType KeltnerMaType { get; set; }

        [Parameter("ATR Period", Group = "Keltner Channels", DefaultValue = 20)]
        public int AtrPeriods { get; set; }

        [Parameter("ATR MA Type", Group = "Keltner Channels", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType AtrMaType { get; set; }

        [Parameter("BandDistance", Group = "Keltner Channels", DefaultValue = 1.5)]
        public double BandDistance { get; set; }

        [Parameter("Momentum Periods", DefaultValue = 12)]
        public int MomentumPeriods { get; set; }

        [Output("Up", LineColor = "ForestGreen", PlotType = PlotType.Histogram, Thickness = 4)]
        public IndicatorDataSeries Up { get; set; }

        [Output("Down", LineColor = "Red", PlotType = PlotType.Histogram, Thickness = 4)]
        public IndicatorDataSeries Down { get; set; }

        [Output("Momentum", LineColor = "Yellow", LineStyle = LineStyle.Solid, Thickness = 1)]
        public IndicatorDataSeries Momentum { get; set; }

        private BollingerBands BbInd;
        private KeltnerChannels KcInd;

        private double KeltnerUp;
        private double KeltnerDown;
        private double BollingerUp;
        private double BollingerDown;

        protected override void Initialize()
        {
            BbInd = Indicators.BollingerBands(Bars.ClosePrices, BollingerPeriod, StDeviation, BollingerMaType);
            KcInd = Indicators.KeltnerChannels(KeltnerPeriod, KeltnerMaType, AtrPeriods, AtrMaType, BandDistance);
        }

        public override void Calculate(int index)
        {
            BollingerUp = BbInd.Top[index];
            BollingerDown = BbInd.Bottom[index];
            KeltnerUp = KcInd.Top[index];
            KeltnerDown = KcInd.Bottom[index];

            Momentum[index] = Bars.ClosePrices[index] - Bars.ClosePrices[index - MomentumPeriods];

            if (BollingerUp >= KeltnerUp || BollingerDown <= KeltnerDown)
            {
                Up[index] = Math.Abs(BollingerUp - KeltnerUp) + Math.Abs(BollingerDown - KeltnerDown);
            }
            else
            {
                Up[index] = 0;
            }

            if (BollingerUp < KeltnerUp && BollingerDown > KeltnerDown)
            {
                Down[index] = -(Math.Abs(BollingerUp - KeltnerUp) + Math.Abs(BollingerDown - KeltnerDown));
            }
            else
            {
                Down[index] = 0;
            }
        }

    }
}
