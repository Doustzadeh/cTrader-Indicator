using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Levels(0)]
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class KnowSureThing : Indicator
    {
        // RCMA1 = 10-Period SMA of 10-Period Rate-of-Change 
        // RCMA2 = 10-Period SMA of 15-Period Rate-of-Change 
        // RCMA3 = 10-Period SMA of 20-Period Rate-of-Change 
        // RCMA4 = 15-Period SMA of 30-Period Rate-of-Change 
        // KST = (RCMA1 x 1) + (RCMA2 x 2) + (RCMA3 x 3) + (RCMA4 x 4)
        // Signal Line = 9-period SMA of KST

        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter("ROC1 Periods", Group = "ROC", DefaultValue = 10)]
        public int RocPeriods1 { get; set; }

        [Parameter("ROC2 Periods", Group = "ROC", DefaultValue = 15)]
        public int RocPeriods2 { get; set; }

        [Parameter("ROC3 Periods", Group = "ROC", DefaultValue = 20)]
        public int RocPeriods3 { get; set; }

        [Parameter("ROC4 Periods", Group = "ROC", DefaultValue = 30)]
        public int RocPeriods4 { get; set; }

        [Parameter("SMA1 Periods", Group = "SMA", DefaultValue = 10)]
        public int SmaPeriods1 { get; set; }

        [Parameter("SMA2 Periods", Group = "SMA", DefaultValue = 10)]
        public int SmaPeriods2 { get; set; }

        [Parameter("SMA3 Periods", Group = "SMA", DefaultValue = 10)]
        public int SmaPeriods3 { get; set; }

        [Parameter("SMA4 Periods", Group = "SMA", DefaultValue = 15)]
        public int SmaPeriods4 { get; set; }

        [Parameter("Signal Periods", DefaultValue = 9, MinValue = 1)]
        public int SignalPeriods { get; set; }

        [Output("KST", LineColor = "DodgerBlue")]
        public IndicatorDataSeries KST { get; set; }

        [Output("Signal", LineColor = "Red")]
        public IndicatorDataSeries Signal { get; set; }

        private PriceROC Roc1, Roc2, Roc3, Roc4;
        private SimpleMovingAverage RCMA1, RCMA2, RCMA3, RCMA4;
        private SimpleMovingAverage SignalSMA;

        protected override void Initialize()
        {
            Roc1 = Indicators.PriceROC(Source, RocPeriods1);
            Roc2 = Indicators.PriceROC(Source, RocPeriods2);
            Roc3 = Indicators.PriceROC(Source, RocPeriods3);
            Roc4 = Indicators.PriceROC(Source, RocPeriods4);

            RCMA1 = Indicators.SimpleMovingAverage(Roc1.Result, SmaPeriods1);
            RCMA2 = Indicators.SimpleMovingAverage(Roc2.Result, SmaPeriods2);
            RCMA3 = Indicators.SimpleMovingAverage(Roc3.Result, SmaPeriods3);
            RCMA4 = Indicators.SimpleMovingAverage(Roc4.Result, SmaPeriods4);

            SignalSMA = Indicators.SimpleMovingAverage(KST, SignalPeriods);
        }

        public override void Calculate(int index)
        {
            KST[index] = 1 * RCMA1.Result[index] + 2 * RCMA2.Result[index] + 3 * RCMA3.Result[index] + 4 * RCMA4.Result[index];
            Signal[index] = SignalSMA.Result[index];
        }

    }
}
