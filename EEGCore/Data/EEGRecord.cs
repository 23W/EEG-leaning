using EEGCore.Processing.Model;
using static System.Net.WebRequestMethods;

namespace EEGCore.Data
{
    public enum LeadType
    {
        Unknown,
        Frontal,
        Central,
        Temporal,
        Parietal,
        Occipital,
    }

    public enum LeadCode
    {
        // Fp line (from left to right)
        Fp1,
        Fpz,
        Fp2,

        // AF line (from left to right)
        AF7,
        AF3,
        AFz,
        AF4,
        AF8,

        // F line (from left to right)
        F7,
        F3,
        Fz,
        F4,
        F8,

        // FC line (from left to right)
        FT7,
        FC5,
        FC3,
        FCz,
        FC4,
        FC6,
        FT8,

        // T line (from left to right)
        T3,
        C3,
        Cz,
        C4,
        T4,

        // P line (from left to right)
        T5,
        P3,
        Pz,
        P4,
        T6,

        // O line (from left to right)
        O1,
        Oz,
        O2
    }

    // Spherical EEG coordinates in degrees
    public class EEGCoordinate
    {
        public double Alpha { get; set; } = 0;
        public double Beta { get; set; } = 0;
        public double R { get; set; } = 1;
    }

    public class EEGSchemeLeadInfo
    {
        public LeadType LeadType { get; set; } = LeadType.Unknown;

        public EEGCoordinate Coordinate { get; set; } = new EEGCoordinate();
    }

    public static class EEGScheme
    {
        public static Dictionary<LeadCode, EEGSchemeLeadInfo> Scheme1020 { get; } = new Dictionary<LeadCode, EEGSchemeLeadInfo>()
        {
            // Fp line (from left to right)
            {
                LeadCode.Fp1,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.Fp1) }
            },
            {
                LeadCode.Fpz,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.Fpz) }
            },
            {
                LeadCode.Fp2,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.Fp2) }
            },

            // AF line (from left to right)
            {
                LeadCode.AF7,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.AF7) }
            },
            {
                LeadCode.AF3,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.AF3) }
            },
            {
                LeadCode.AFz,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.AFz) }
            },
            {
                LeadCode.AF4,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.AF4) }
            },
            {
                LeadCode.AF8,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.AF8) }
            },

            // F line (from left to right)
            {
                LeadCode.F7,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.F7) }
            },
            {
                LeadCode.F3,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.F3) }
            },
            {
                LeadCode.Fz,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.Fz) }
            },
            {
                LeadCode.F4,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.F4) }
            },
            {
                LeadCode.F8,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.F8) }
            },

            // FC line (from left to right)
            {
                LeadCode.FT7,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.FT7) }
            },
            {
                LeadCode.FC5,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.FC5) }
            },
            {
                LeadCode.FC3,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.FC3) }
            },
            {
                LeadCode.FCz,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.FCz) }
            },
            {
                LeadCode.FC4,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.FC4) }
            },
            {
                LeadCode.FC6,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.FC6) }
            },
            {
                LeadCode.FT8,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Frontal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.FT8) }
            },

            // T line (from left to right)
            {
                LeadCode.T3,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Temporal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.T3) }
            },
            {
                LeadCode.C3,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Central, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.C3) }
            },
            {
                LeadCode.Cz,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Central, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.Cz) }
            },
            {
                LeadCode.C4,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Central, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.C4) }
            },
            {
                LeadCode.T4,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Temporal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.T4) }
            },

            // P line (from left to right)
            {
                LeadCode.T5,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Temporal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.T5) }
            },
            {
                LeadCode.P3,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Parietal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.P3) }
            },
            {
                LeadCode.Pz,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Parietal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.Pz) }
            },
            {
                LeadCode.P4,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Parietal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.P4) }
            },
            {
                LeadCode.T6,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Temporal, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.T6) }
            },

            // O line (from left to right)
            {
                LeadCode.O1,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Occipital, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.O1) }
            },
            {
                LeadCode.Oz,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Occipital, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.Oz) }
            },
            {
                LeadCode.O2,
                new EEGSchemeLeadInfo() { LeadType = LeadType.Occipital, Coordinate = ScalpSphere.GetLeadSpherical(LeadCode.O2) }
            },
        };
    }

    public class EEGLead : Lead
    {
        public LeadType LeadType { get; set; } = LeadType.Unknown;
    }
}
