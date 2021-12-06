using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Levels(0)]
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class SpecialK : Indicator
    {
        // Special K = 10 Period Simple Moving Average of ROC(10) * 1
        //           + 10 Period Simple Moving Average of ROC(15) * 2
        //           + 10 Period Simple Moving Average of ROC(20) * 3
        //           + 15 Period Simple Moving Average of ROC(30) * 4
        //           + 50 Period Simple Moving Average of ROC(40) * 1
        //           + 65 Period Simple Moving Average of ROC(65) * 2
        //           + 75 Period Simple Moving Average of ROC(75) * 3
        //           +100 Period Simple Moving Average of ROC(100)* 4
        //           +130 Period Simple Moving Average of ROC(195)* 1
        //           +130 Period Simple Moving Average of ROC(265)* 2
        //           +130 Period Simple Moving Average of ROC(390)* 3
        //           +195 Period Simple Moving Average of ROC(530)* 4

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

        [Parameter("ROC5 Periods", Group = "ROC", DefaultValue = 40)]
        public int RocPeriods5 { get; set; }

        [Parameter("ROC6 Periods", Group = "ROC", DefaultValue = 65)]
        public int RocPeriods6 { get; set; }

        [Parameter("ROC7 Periods", Group = "ROC", DefaultValue = 75)]
        public int RocPeriods7 { get; set; }

        [Parameter("ROC8 Periods", Group = "ROC", DefaultValue = 100)]
        public int RocPeriods8 { get; set; }

        [Parameter("ROC9 Periods", Group = "ROC", DefaultValue = 195)]
        public int RocPeriods9 { get; set; }

        [Parameter("ROC10 Periods", Group = "ROC", DefaultValue = 265)]
        public int RocPeriods10 { get; set; }

        [Parameter("ROC11 Periods", Group = "ROC", DefaultValue = 390)]
        public int RocPeriods11 { get; set; }

        [Parameter("ROC12 Periods", Group = "ROC", DefaultValue = 530)]
        public int RocPeriods12 { get; set; }

        [Parameter("SMA1 Periods", Group = "SMA", DefaultValue = 10)]
        public int SmaPeriods1 { get; set; }

        [Parameter("SMA2 Periods", Group = "SMA", DefaultValue = 10)]
        public int SmaPeriods2 { get; set; }

        [Parameter("SMA3 Periods", Group = "SMA", DefaultValue = 10)]
        public int SmaPeriods3 { get; set; }

        [Parameter("SMA4 Periods", Group = "SMA", DefaultValue = 15)]
        public int SmaPeriods4 { get; set; }

        [Parameter("SMA5 Periods", Group = "SMA", DefaultValue = 50)]
        public int SmaPeriods5 { get; set; }

        [Parameter("SMA6 Periods", Group = "SMA", DefaultValue = 65)]
        public int SmaPeriods6 { get; set; }

        [Parameter("SMA7 Periods", Group = "SMA", DefaultValue = 75)]
        public int SmaPeriods7 { get; set; }

        [Parameter("SMA8 Periods", Group = "SMA", DefaultValue = 100)]
        public int SmaPeriods8 { get; set; }

        [Parameter("SMA9 Periods", Group = "SMA", DefaultValue = 130)]
        public int SmaPeriods9 { get; set; }

        [Parameter("SMA10 Periods", Group = "SMA", DefaultValue = 130)]
        public int SmaPeriods10 { get; set; }

        [Parameter("SMA11 Periods", Group = "SMA", DefaultValue = 130)]
        public int SmaPeriods11 { get; set; }

        [Parameter("SMA12 Periods", Group = "SMA", DefaultValue = 195)]
        public int SmaPeriods12 { get; set; }

        [Parameter("Signal Periods", DefaultValue = 9, MinValue = 1)]
        public int SignalPeriods { get; set; }

        [Output("Special K", LineColor = "DodgerBlue")]
        public IndicatorDataSeries SK { get; set; }

        [Output("Signal", LineColor = "Red")]
        public IndicatorDataSeries Signal { get; set; }

        private PriceROC Roc1, Roc2, Roc3, Roc4, Roc5, Roc6, Roc7, Roc8, Roc9, Roc10,
        Roc11, Roc12;
        private SimpleMovingAverage RCMA1, RCMA2, RCMA3, RCMA4, RCMA5, RCMA6, RCMA7, RCMA8, RCMA9, RCMA10,
        RCMA11, RCMA12;
        private SimpleMovingAverage SignalSMA;

        protected override void Initialize()
        {
            Roc1 = Indicators.PriceROC(Source, RocPeriods1);
            Roc2 = Indicators.PriceROC(Source, RocPeriods2);
            Roc3 = Indicators.PriceROC(Source, RocPeriods3);
            Roc4 = Indicators.PriceROC(Source, RocPeriods4);
            Roc5 = Indicators.PriceROC(Source, RocPeriods5);
            Roc6 = Indicators.PriceROC(Source, RocPeriods6);
            Roc7 = Indicators.PriceROC(Source, RocPeriods7);
            Roc8 = Indicators.PriceROC(Source, RocPeriods8);
            Roc9 = Indicators.PriceROC(Source, RocPeriods9);
            Roc10 = Indicators.PriceROC(Source, RocPeriods10);
            Roc11 = Indicators.PriceROC(Source, RocPeriods11);
            Roc12 = Indicators.PriceROC(Source, RocPeriods12);

            RCMA1 = Indicators.SimpleMovingAverage(Roc1.Result, SmaPeriods1);
            RCMA2 = Indicators.SimpleMovingAverage(Roc2.Result, SmaPeriods2);
            RCMA3 = Indicators.SimpleMovingAverage(Roc3.Result, SmaPeriods3);
            RCMA4 = Indicators.SimpleMovingAverage(Roc4.Result, SmaPeriods4);
            RCMA5 = Indicators.SimpleMovingAverage(Roc5.Result, SmaPeriods5);
            RCMA6 = Indicators.SimpleMovingAverage(Roc6.Result, SmaPeriods6);
            RCMA7 = Indicators.SimpleMovingAverage(Roc7.Result, SmaPeriods7);
            RCMA8 = Indicators.SimpleMovingAverage(Roc8.Result, SmaPeriods8);
            RCMA9 = Indicators.SimpleMovingAverage(Roc9.Result, SmaPeriods9);
            RCMA10 = Indicators.SimpleMovingAverage(Roc10.Result, SmaPeriods10);
            RCMA11 = Indicators.SimpleMovingAverage(Roc11.Result, SmaPeriods11);
            RCMA12 = Indicators.SimpleMovingAverage(Roc12.Result, SmaPeriods12);

            SignalSMA = Indicators.SimpleMovingAverage(SK, SignalPeriods);
        }

        public override void Calculate(int index)
        {
            SK[index] = 1 * RCMA1.Result[index] + 2 * RCMA2.Result[index] + 3 * RCMA3.Result[index] + 4 * RCMA4.Result[index] + 1 * RCMA5.Result[index] + 2 * RCMA6.Result[index] + 3 * RCMA7.Result[index] + 4 * RCMA8.Result[index] + 1 * RCMA9.Result[index] + 2 * RCMA10.Result[index] + 3 * RCMA11.Result[index] + 4 * RCMA12.Result[index];

            Signal[index] = SignalSMA.Result[index];
        }

    }
}
