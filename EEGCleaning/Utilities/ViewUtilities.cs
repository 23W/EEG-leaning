using EEGCore.Data;
using OxyPlot;

namespace EEGCleaning.Utilities
{
    internal static class ViewUtilities
    {
        internal static OxyColor GetLeadColor(Lead lead) => GetLeadColor(lead.LeadType);

        internal static OxyColor GetLeadColor(LeadType lead)
        {
            var res = OxyColors.DarkGray;

            switch (lead)
            {
                case LeadType.Frontal: res = OxyColors.DarkBlue; break;
                case LeadType.Central: res = OxyColors.DarkGreen; break;
                case LeadType.Parietal: res = OxyColors.DarkOliveGreen; break;
                case LeadType.Temporal: res = OxyColors.DarkOrange; break;
                case LeadType.Occipital: res = OxyColors.DarkRed; break;
                case LeadType.IndependenctComponent: res = OxyColors.DarkOrange; break;
            }

            return res;
        }
    }
}
