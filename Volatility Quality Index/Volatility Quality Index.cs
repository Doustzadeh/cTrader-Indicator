using System;
using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class VolatilityQualityIndex : Indicator
    {
        [Parameter("Fast Periods", DefaultValue = 9)]
        public int FastPeriods { get; set; }

        [Parameter("Slow Periods", DefaultValue = 200)]
        public int SlowPeriods { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType MaType { get; set; }

        [Output("VQI", LineColor = "Lime", Thickness = 2)]
        public IndicatorDataSeries VQI { get; set; }

        [Output("Fast MA", LineColor = "DodgerBlue")]
        public IndicatorDataSeries FastMA { get; set; }

        [Output("Slow MA", LineColor = "Red")]
        public IndicatorDataSeries SlowMA { get; set; }

        private double vqi_t;
        private IndicatorDataSeries vqi;
        private TrueRange TR;
        private MovingAverage MA_Fast, MA_Slow;


        protected override void Initialize()
        {
            vqi = CreateDataSeries();
            TR = Indicators.TrueRange();
            MA_Fast = Indicators.MovingAverage(VQI, FastPeriods, MaType);
            MA_Slow = Indicators.MovingAverage(VQI, SlowPeriods, MaType);
        }

        public override void Calculate(int index)
        {
            if (index < 1)
            {
                vqi[index] = 0;
                VQI[index] = 0;
                return;
            }

            double PrvClose = Bars.ClosePrices[index - 1];

            double Open = Bars.OpenPrices[index];
            double High = Bars.HighPrices[index];
            double Low = Bars.LowPrices[index];
            double Close = Bars.ClosePrices[index];

            double TrueRange = TR.Result[index];
            double Range = High - Low;

            vqi_t = TrueRange != 0 && Range != 0 ? (((Close - PrvClose) / TrueRange) + ((Close - Open) / Range)) * 0.5 : vqi[index - 1];
            vqi[index] = Math.Abs(vqi_t) * ((Close - PrvClose + Close - Open) * 0.5);

            VQI[index] = VQI[index - 1] + vqi[index];
            FastMA[index] = MA_Fast.Result[index];
            SlowMA[index] = MA_Slow.Result[index];
        }
    }
}
