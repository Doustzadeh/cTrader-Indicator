using System;
using cAlgo.API;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class VariableMovingAverage : Indicator
    {
        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter("VMA Periods", DefaultValue = 6, MinValue = 2)]
        public int Periods { get; set; }

        [Output("VMA", LineColor = "Gold", LineStyle = LineStyle.Solid, Thickness = 2)]
        public IndicatorDataSeries VMA { get; set; }

        private IndicatorDataSeries pdmS, mdmS, pdiS, mdiS, iS;

        protected override void Initialize()
        {
            pdmS = CreateDataSeries();
            mdmS = CreateDataSeries();
            pdiS = CreateDataSeries();
            mdiS = CreateDataSeries();
            iS = CreateDataSeries();
        }

        public override void Calculate(int index)
        {
            if (index < 1)
            {
                pdmS[index] = 0;
                mdmS[index] = 0;
                pdiS[index] = 0;
                mdiS[index] = 0;
                iS[index] = 0;
                VMA[index] = 0;
                return;
            }

            double k = 1.0 / Periods;

            double pdm = Math.Max((Source[index] - Source[index - 1]), 0);
            double mdm = Math.Max((Source[index - 1] - Source[index]), 0);

            pdmS[index] = ((1 - k) * pdmS[index - 1]) + (k * pdm);
            mdmS[index] = ((1 - k) * mdmS[index - 1]) + (k * mdm);

            double s = pdmS[index] + mdmS[index];
            double pdi = pdmS[index] / s;
            double mdi = mdmS[index] / s;

            pdiS[index] = ((1 - k) * pdiS[index - 1]) + (k * pdi);
            mdiS[index] = ((1 - k) * mdiS[index - 1]) + (k * mdi);

            double d = Math.Abs(pdiS[index] - mdiS[index]);
            double s1 = pdiS[index] + mdiS[index];

            iS[index] = ((1 - k) * iS[index - 1]) + (k * d / s1);

            double hhv = iS.Maximum(Periods);
            double llv = iS.Minimum(Periods);
            double dif = hhv - llv;
            double vI = (iS[index] - llv) / dif;

            VMA[index] = ((1 - (k * vI)) * VMA[index - 1]) + (k * vI * Source[index]);
        }

    }
}
