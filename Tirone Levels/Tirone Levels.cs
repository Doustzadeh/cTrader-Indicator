using cAlgo.API;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class TironeLevels : Indicator
    {
        [Parameter("Midpoint Method", Group = "Method", DefaultValue = true)]
        public bool Midpoint { get; set; }

        [Parameter("Mean Method", Group = "Method", DefaultValue = false)]
        public bool Mean { get; set; }

        [Parameter("Periods", DefaultValue = 20)]
        public int Periods { get; set; }

        [Output("Top Line", LineColor = "Lime", LineStyle = LineStyle.LinesDots)]
        public IndicatorDataSeries TopLine { get; set; }

        [Output("Center Line", LineColor = "Gold", LineStyle = LineStyle.LinesDots)]
        public IndicatorDataSeries CenterLine { get; set; }

        [Output("Bottom Line", LineColor = "Red", LineStyle = LineStyle.LinesDots)]
        public IndicatorDataSeries BottomLine { get; set; }

        [Output("Extreme High", LineColor = "Lime")]
        public IndicatorDataSeries ExtremeHigh { get; set; }

        [Output("Regular High", LineColor = "Lime")]
        public IndicatorDataSeries RegularHigh { get; set; }

        [Output("Adjusted Mean", LineColor = "Gold")]
        public IndicatorDataSeries AdjustedMean { get; set; }

        [Output("Regular Low", LineColor = "Red")]
        public IndicatorDataSeries RegularLow { get; set; }

        [Output("Extreme Low", LineColor = "Red")]
        public IndicatorDataSeries ExtremeLow { get; set; }

        public override void Calculate(int index)
        {
            double HighestHigh = Bars.HighPrices.Maximum(Periods);
            double LowestLow = Bars.LowPrices.Minimum(Periods);

            // Midpoint method
            if (Midpoint)
            {
                TopLine[index] = HighestHigh - ((HighestHigh - LowestLow) / 3);
                CenterLine[index] = LowestLow + ((HighestHigh - LowestLow) / 2);
                BottomLine[index] = LowestLow + ((HighestHigh - LowestLow) / 3);
            }

            // Mean method
            if (Mean)
            {
                AdjustedMean[index] = (HighestHigh + LowestLow + Bars.ClosePrices[index]) / 3;

                ExtremeHigh[index] = AdjustedMean[index] + (HighestHigh - LowestLow);
                RegularHigh[index] = (2 * AdjustedMean[index]) - LowestLow;

                ExtremeLow[index] = AdjustedMean[index] - (HighestHigh - LowestLow);
                RegularLow[index] = (2 * AdjustedMean[index]) - HighestHigh;
            }
        }
    }
}
