using cAlgo.API;

namespace cAlgo
{
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class AccumulationDistributionLine : Indicator
    {
        [Output("ADL", LineColor = "Red")]
        public IndicatorDataSeries ADL { get; set; }

        // 1. Money Flow Multiplier = [(Close - Low) - (High - Close)] / (High - Low)
        // 2. Money Flow Volume = Money Flow Multiplier x Volume for the Period
        // 3. ADL = Previous ADL + Current Period's Money Flow Volume

        private double High, Low, Close, Volume, MFM, MFV;

        public override void Calculate(int index)
        {
            // High = High price for the period
            // Low = Low price for the period
            // Close = Closing price
            // Volume = Volume for the Period
            // MFM = Money Flow Multiplier
            // MFV = Money Flow Volume

            if (index < 1)
            {
                ADL[index] = 0;
                return;
            }

            High = Bars.HighPrices[index];
            Low = Bars.LowPrices[index];
            Close = Bars.ClosePrices[index];
            Volume = Bars.TickVolumes[index];

            MFM = High - Low == 0 ? 0 : ((Close - Low) - (High - Close)) / (High - Low);
            MFV = MFM * Volume;

            ADL[index] = ADL[index - 1] + MFV;
        }

    }
}
