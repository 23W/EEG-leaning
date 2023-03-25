using EEGCore.Data;
using OxyPlot;

namespace EEGCleaning.Utilities
{
    internal static class ViewUtilities
    {
        internal static OxyColor DefaultLeadColor => OxyColors.DarkGray;

        internal static OxyColor GetLeadColor(Lead lead)
        {
            var res = DefaultLeadColor;

            if (lead is EEGLead eegLead)
            {
                res = GetLeadColor(eegLead.LeadType);
            }
            else if (lead is ComponentLead componentLead)
            {
                res = (componentLead.Suppress == SuppressType.None) ? OxyColors.DarkOrange : OxyColors.DarkGray;
            }

            return res;
        }

        internal static OxyColor GetAlternativeLeadColor(Lead lead)
        {
            var res = DefaultLeadColor;

            if (lead is EEGLead eegLead)
            {
                res = OxyColors.DarkGray;
            }
            else if (lead is ComponentLead componentLead)
            {
                res = (componentLead.Suppress == SuppressType.None) ? OxyColors.DarkRed: OxyColors.DarkSlateBlue;
            }

            return res;
        }

        internal static OxyColor GetLeadColor(LeadType lead)
        {
            var res = DefaultLeadColor;

            switch (lead)
            {
                case LeadType.Frontal: res = OxyColors.DarkBlue; break;
                case LeadType.Central: res = OxyColors.DarkGreen; break;
                case LeadType.Parietal: res = OxyColors.DarkOliveGreen; break;
                case LeadType.Temporal: res = OxyColors.DarkOrange; break;
                case LeadType.Occipital: res = OxyColors.DarkRed; break;
            }

            return res;
        }

        internal static PointF GetDPI(Control control)
        {
            PointF dpi = PointF.Empty;

            using (Graphics g = control.CreateGraphics())
            {
                dpi.X = g.DpiX;
                dpi.Y = g.DpiY;
            }

            return dpi;
        }

        internal static PointF GetDPMM(Control control)
        {
            PointF dpi = GetDPI(control);
            dpi = new PointF(dpi.X / 25.4f, dpi.Y / 25.4f);

            return dpi;
        }
    }
}
