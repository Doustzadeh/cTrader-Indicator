using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class TTMSqueeze : Indicator
    {
        [Parameter("Periods", Group = "BollingerBands", DefaultValue = 20)]
        public int BollingerPeriod { get; set; }

        [Parameter("Standard Dev", Group = "BollingerBands", DefaultValue = 2, MinValue = 0)]
        public double StDeviation { get; set; }

        [Parameter("MA Type", Group = "BollingerBands", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType BollingerMaType { get; set; }

        [Parameter("MA Periods", Group = "KeltnerChannels", DefaultValue = 20)]
        public int KeltnerPeriod { get; set; }

        [Parameter("MA Type", Group = "KeltnerChannels", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType KeltnerMaType { get; set; }

        [Parameter("ATR Periods", Group = "KeltnerChannels", DefaultValue = 20)]
        public int AtrPeriods { get; set; }

        [Parameter("ATR MA Type", Group = "KeltnerChannels", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType AtrMaType { get; set; }

        [Parameter("Band Distance", Group = "KeltnerChannels", DefaultValue = 1.5, MinValue = 0)]
        public double BandDistance { get; set; }

        [Parameter("SMA Periods", DefaultValue = 20)]
        public int SmaPeriods { get; set; }

        [Parameter("LRS Periods", DefaultValue = 20)]
        public int LrsPeriods { get; set; }

        [Output("Up Rising", LineColor = "#FF3B65A8", PlotType = PlotType.Histogram, Thickness = 3)]
        public IndicatorDataSeries UpRising { get; set; }

        [Output("Up Falling", LineColor = "#FFADCDFD", PlotType = PlotType.Histogram, Thickness = 3)]
        public IndicatorDataSeries UpFalling { get; set; }

        [Output("Down Rising", LineColor = "#FFFBAEBF", PlotType = PlotType.Histogram, Thickness = 3)]
        public IndicatorDataSeries DownRising { get; set; }

        [Output("Down Falling", LineColor = "#FFFA4347", PlotType = PlotType.Histogram, Thickness = 3)]
        public IndicatorDataSeries DownFalling { get; set; }

        [Output("Squeeze On", LineColor = "Red", PlotType = PlotType.Points, Thickness = 5)]
        public IndicatorDataSeries SqueezeOn { get; set; }

        [Output("Squeeze Off", LineColor = "Lime", PlotType = PlotType.Points, Thickness = 5)]
        public IndicatorDataSeries SqueezeOff { get; set; }

        private IndicatorDataSeries Midline, Delta;
        private BollingerBands BbInd;
        private KeltnerChannels KcInd;
        private double KeltnerUp, KeltnerDown, BollingerUp, BollingerDown;
        private SimpleMovingAverage SMA;
        private LinearRegressionSlope LRS;

        protected override void Initialize()
        {
            Midline = CreateDataSeries();
            Delta = CreateDataSeries();
            BbInd = Indicators.BollingerBands(Bars.ClosePrices, BollingerPeriod, StDeviation, BollingerMaType);
            KcInd = Indicators.KeltnerChannels(KeltnerPeriod, KeltnerMaType, AtrPeriods, AtrMaType, BandDistance);
            SMA = Indicators.SimpleMovingAverage(Bars.ClosePrices, SmaPeriods);
            LRS = Indicators.LinearRegressionSlope(Delta, LrsPeriods);
        }

        public override void Calculate(int index)
        {
            BollingerUp = BbInd.Top[index];
            BollingerDown = BbInd.Bottom[index];
            KeltnerUp = KcInd.Top[index];
            KeltnerDown = KcInd.Bottom[index];

            if (BollingerUp < KeltnerUp && BollingerDown > KeltnerDown)
            {
                SqueezeOn[index] = 0;
            }
            else
            {
                SqueezeOff[index] = 0;
            }

            Midline[index] = (Bars.HighPrices.Maximum(20) + Bars.LowPrices.Minimum(20)) / 2;
            Delta[index] = Bars.ClosePrices[index] - ((Midline[index] + SMA.Result[index]) / 2);

            if (LRS.Result[index] > 0)
            {
                if (LRS.Result.IsRising())
                {
                    UpRising[index] = LRS.Result[index];
                }
                else
                {
                    UpFalling[index] = LRS.Result[index];
                }
            }
            else
            {
                if (LRS.Result.IsRising())
                {
                    DownRising[index] = LRS.Result[index];
                }
                else
                {
                    DownFalling[index] = LRS.Result[index];
                }
            }
        }
    }
}
