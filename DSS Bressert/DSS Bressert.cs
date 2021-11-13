using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Levels(0, 20, 80, 100)]
    [Indicator(IsOverlay = false, ScalePrecision = 2, AccessRights = AccessRights.None)]
    public class DSSBressert : Indicator
    {
        [Parameter("Stochastic Periods", DefaultValue = 13)]
        public int StochasticPeriods { get; set; }

        [Parameter("EMA Periods", DefaultValue = 8)]
        public int EmaPeriods { get; set; }

        [Parameter("WMA Periods", DefaultValue = 5)]
        public int WmaPeriods { get; set; }

        [Output("DSS", LineColor = "White", Thickness = 1)]
        public IndicatorDataSeries DSS { get; set; }

        [Output("DSS Up", LineColor = "DodgerBlue", PlotType = PlotType.Points, Thickness = 5)]
        public IndicatorDataSeries DssUp { get; set; }

        [Output("DSS Down", LineColor = "Red", PlotType = PlotType.Points, Thickness = 5)]
        public IndicatorDataSeries DssDown { get; set; }

        [Output("WMA", LineColor = "Gold", Thickness = 1)]
        public IndicatorDataSeries WmaResult { get; set; }

        private double Ln = 0;
        private double Hn = 0;
        private double LXn = 0;
        private double HXn = 0;
        private double alpha = 0;
        private IndicatorDataSeries mit;
        private WeightedMovingAverage WMA;

        protected override void Initialize()
        {
            mit = CreateDataSeries();
            alpha = 2.0 / (1.0 + EmaPeriods);
            WMA = Indicators.WeightedMovingAverage(DSS, WmaPeriods);
        }

        public override void Calculate(int index)
        {
            if (double.IsNaN(mit[index - 1]))
            {
                mit[index - 1] = 0;
            }

            if (double.IsNaN(DSS[index - 1]))
            {
                DSS[index - 1] = 0;
            }

            Ln = Bars.LowPrices.Minimum(StochasticPeriods);
            Hn = Bars.HighPrices.Maximum(StochasticPeriods);

            mit[index] = mit[index - 1] + alpha * ((((Bars.ClosePrices[index] - Ln) / (Hn - Ln)) * 100) - mit[index - 1]);

            LXn = mit.Minimum(StochasticPeriods);
            HXn = mit.Maximum(StochasticPeriods);
            DSS[index] = DSS[index - 1] + alpha * ((((mit[index] - LXn) / (HXn - LXn)) * 100) - DSS[index - 1]);

            if (DSS[index] > DSS[index - 1])
            {
                DssUp[index] = DSS[index];
                DssDown[index] = double.NaN;
            }

            if (DSS[index] < DSS[index - 1])
            {
                DssDown[index] = DSS[index];
                DssUp[index] = double.NaN;
            }

            WmaResult[index] = WMA.Result[index];
        }

    }
}
