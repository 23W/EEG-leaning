using EEGCore.Data;

namespace EEGCore.Utilities
{
    internal static class DataUtilities
    {
        internal static LeadType GetLeadType(string leadName)
        {
            var leadType = LeadType.Unknown;

            if (leadName.StartsWith("AF", StringComparison.OrdinalIgnoreCase) ||
                leadName.StartsWith("F", StringComparison.OrdinalIgnoreCase))
            {
                leadType = LeadType.Frontal;
            }
            else if (leadName.StartsWith("T", StringComparison.OrdinalIgnoreCase))
            {
                leadType = LeadType.Temporal;
            }
            else if (leadName.StartsWith("C", StringComparison.OrdinalIgnoreCase))
            {
                leadType = LeadType.Central;
            }
            else if (leadName.StartsWith("P", StringComparison.OrdinalIgnoreCase))
            {
                leadType = LeadType.Parietal;
            }
            else if (leadName.StartsWith("O", StringComparison.OrdinalIgnoreCase))
            {
                leadType = LeadType.Occipital;
            }

            return leadType;
        }

        internal static int ComparetTo(Lead l1, Lead l2)
        {
            var res = (l1.LeadType - l2.LeadType);
            if (res == 0)
            {
                res = l1.Name.CompareTo(l2.Name);
            }

            return res;
        }
    }
}
